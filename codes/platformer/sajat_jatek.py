import os
import sys
import random
import math
import pygame
from os import listdir
from os.path import isfile, join

pygame.init()

pygame.display.set_caption("Platformer")

WIDTH, HEGIHT = 1000, 800
FPS = 75
PLAYER_VEL = 5

window = pygame.display.set_mode((WIDTH,HEGIHT))
#sprites--------------------------
def flip(sprites):
    return [pygame.transform.flip(sprite,True,False) for sprite in sprites]

def load_sprite_sheets(dir1,dir2,width,height,direction=False):
    path = join("assets",dir1,dir2)
    images = [f for f in listdir(path) if isfile(join(path,f))]#load every single file that is inside *this* directory

    all_sprites = {}
    for image in images:
        sprite_sheet = pygame.image.load(join(path,image)).convert_alpha()

        sprites = []
        for i in range(sprite_sheet.get_width()//width):
            surface = pygame.Surface((width,height), pygame.SRCALPHA,32)
            rect = pygame.Rect(i * width,0,width,height)
            surface.blit(sprite_sheet,(0,0),rect) # draw (what, destination, mire)
            sprites.append(pygame.transform.scale2x(surface))
        #directions
        if direction:
            all_sprites[image.replace(".png","")+"_right"] = sprites
            all_sprites[image.replace(".png","")+"_left"] = flip(sprites)
        else:
            all_sprites[image.replace(".png","")] = sprites
    return all_sprites
        
#--------------------------sprites
def get_block(size):
    path = join("assets", "Terrain", "Terrain.png")
    image = pygame.image.load(path).convert_alpha()
    surface = pygame.Surface((size,size),pygame.SRCALPHA, 32)
    rect = pygame.Rect(96,128,size,size)
    surface.blit(image,(0,0),rect)
    return pygame.transform.scale2x(surface)
#player-----------------

class Player(pygame.sprite.Sprite):
    HP = 10
    COLOR =(255,0,0)
    GRAVITY = 1
    SPRITES = load_sprite_sheets("MainCharacters","NinjaFrog",32,32,True)
    ANIMATION_DELAY = 4


    def __init__(self,x,y,width,height):
        super().__init__()
        self.rect = pygame.Rect(x,y,width,height)
        self.x_vel=0
        self.y_vel=0
        self.mask = None
        self.direction = "left"
        self.animation_count = 0
        self.fall_count = 0
        self.jump_count = 0
        self.hit = False
        self.hit_count = 0
        self.hp = self.HP
    
    def jump(self):
        self.y_vel = -self.GRAVITY*8
        self.animation_count = 0
        self.jump_count +=1
        if self.jump_count == 1:
            self.fall_count = 0

    def move(self,dx,dy):
        self.rect.x+=dx
        self.rect.y+=dy

    def make_hit(self):
        self.hit = True
        self.hit_count = 0

    def move_left(self,vel):
        self.x_vel = -vel
        if self.direction != "left":
            self.direction = "left"
            self.animation_count=0

    def move_right(self,vel):
        self.x_vel = vel
        if self.direction != "right":
            self.direction = "right"
            self.animation_count=0

    def loop(self,fps):
        self.y_vel +=min(1,(self.fall_count/fps)*self.GRAVITY)#gravitáció
        self.move(self.x_vel,self.y_vel)

        if self.hit:
            self.hit_count+=1
        if self.hit_count > fps * 0.6: #hit animacio hossza
            self.hit = False
            self.hit_count = 0

        self.fall_count += 1
        self.update_sprite()
        #if self.hp <= 0:
        #    pygame.quit()

    def landed(self):
        self.fall_count = 0
        self.y_vel=0
        self.jump_count = 0
    def hit_head(self):
        self.count=0
        self.y_vel *=-1

    def update_sprite(self):
        sprite_sheet="idle"
        #if self.x_vel != 0:
        #    sprite_sheet="run"

        if self.hit:
            sprite_sheet = "hit"
        elif self.y_vel < 0:
            if self.jump_count == 1:
                sprite_sheet = "jump"
            elif self.jump_count == 2:
                sprite_sheet = "double_jump"
        elif self.y_vel > self.GRAVITY *2 :
            sprite_sheet = "fall"
        elif self.x_vel!=0:
            sprite_sheet="run"
        
        sprite_sheet_name=sprite_sheet + "_" + self.direction
        sprites = self.SPRITES[sprite_sheet_name]
        sprite_index = (self.animation_count // self.ANIMATION_DELAY)%len(sprites)
        self.sprite = sprites[sprite_index]
        self.animation_count +=1
        self.update()
    
    def update(self):
        self.rect = self.sprite.get_rect(topleft = (self.rect.x, self.rect.y))
        self.mask = pygame.mask.from_surface(self.sprite)
    
    def draw(self,win, offset_x):#self and window
        #pygame.draw.rect(win,self.COLOR,self.rect)
        win.blit(self.sprite, (self.rect.x - offset_x,self.rect.y))

#-----------------player

#object------
class Object(pygame.sprite.Sprite):
    def __init__(self,x,y,width,height,name=None,can_hit=False):
        super().__init__()
        self.rect = pygame.Rect(x,y,width,height)
        self.image = pygame.Surface((width,height),pygame.SRCALPHA)
        self.width = width
        self.height = height
        self.name = name
        self.can_hit = False #eldönti hogy az adott objekt tud-e ütni a playerre
    
    def draw(self,win, offset_x):
        win.blit(self.image,(self.rect.x-offset_x,self.rect.y))

class Block(Object):
    def __init__(self,x,y,size):
        super().__init__(x,y,size,size)
        block = get_block(size)
        self.image.blit(block,(0,0))
        self.mask = pygame.mask.from_surface(self.image)

def get_block(size):
    path = join("assets", "Terrain", "Terrain.png")
    image = pygame.image.load(path).convert_alpha()
    surface = pygame.Surface((size,size),pygame.SRCALPHA, 32)
    rect = pygame.Rect(96,128,size,size)
    surface.blit(image,(0,0),rect)
    return pygame.transform.scale2x(surface)

class Spike(Object):
    DAMAGE = 10
    def __init__(self,x,y,width,height):
        super().__init__(x,y,width,height)
        self.can_hit = True

        self.spike = load_sprite_sheets("Traps","Spikes",width,height)
        self.image = self.spike["Idle"][0]
        self.mask = pygame.mask.from_surface(self.image)
        self.name = "spike"
        self.animation_count = 0
        self.animation_name = "Idle"
        self.dmg = self.DAMAGE

    def loop(self):
        sprites = self.spike[self.animation_name]
        sprite_index = (self.animation_count // 
                        self.ANIMATION_DELAY)%len(sprites)
        self.image = sprites[sprite_index]
        self.animation_count +=1
        #update:
        self.rect = self.image.get_rect(topleft = (self.rect.x, self.rect.y))
        self.mask = pygame.mask.from_surface(self.image)

        if self.animation_count // self.ANIMATION_DELAY > len(sprites):
            self.animation_count = 0


#-------object

#background--------
def get_background(name):
    image = pygame.image.load(join("assets","Background",name))
    _, _, width, height = image.get_rect()
    tiles = []

    for i in range(WIDTH // width + 1):
        for j in range(HEGIHT//height+1):
            pos = (i*width, j*height)
            tiles.append(pos)
    return tiles, image

#--------background
def draw(window, background, bg_image, player, objects, offset_x):
    for tile in background:
        window.blit(bg_image, tile)

    for obj in objects:
        obj.draw(window, offset_x)
    player.draw(window, offset_x)

    pygame.display.update()

#vertical collide-----------------------------------------
def handle_vertical_collision(player,objects,dy):#dy amit most épp elmozogtunk
    collided_objects = []
    for obj in objects:
        if pygame.sprite.collide_mask(player,obj):
            if dy > 0:
                player.rect.bottom = obj.rect.top
                player.landed()
            elif dy < 0 :
                player.rect.top = obj.rect.bottom
                player.hit_head()
            collided_objects.append(obj)
    return collided_objects

#--------------------------------------------vertical collide

#horizontal collide---------------------------

def collide(player,objects ,dx):#hivatalosan horizontal collide akart lenni
    player.move(dx,0)
    player.update()
    collided_object = None
    for obj in objects:
        if pygame.sprite.collide_mask(player,obj):
            collided_object = obj
            break
    player.move(-dx,0)
    player.update()
    return collided_object

#--------------------------------horizontal collide

def handle_move(player, objects):
    keys = pygame.key.get_pressed()

    player.x_vel = 0
    collide_left = collide(player,objects,-PLAYER_VEL*2)
    collide_right = collide(player,objects,PLAYER_VEL*2) 

    if keys[pygame.K_a] and not collide_left:
        player.move_left(PLAYER_VEL)
    if keys[pygame.K_d] and not collide_right:
        player.move_right(PLAYER_VEL)

    vertical_collide = handle_vertical_collision(player,objects,player.y_vel)
    to_check = [collide_left,collide_right, *vertical_collide]
    for obj in to_check:
        if obj and obj.can_hit==True:   
            player.make_hit()
            if obj.name == "spike":
                player.hp-=obj.dmg

def main(window):
    clock = pygame.time.Clock()
    background,bg_image = get_background("Blue.png")

    block_size = 96

    player = Player(100,100,50,50)
    #spike1 = Spike(200,HEGIHT-block_size-32,16,32)
    #spike sornal a for-ban a *3 csak a meret miatt kell azon kivul hanyadik saroktol hanyadik sarokig tartson
    spike_sor1 = [Spike(i*32,HEGIHT-block_size-32,16,32)for i in range(3*3, 10*3)]
    floor = [Block(i*block_size, HEGIHT-block_size, block_size) 
             for i in range(-WIDTH*2 // block_size, WIDTH * 3 // block_size)]
    #megrajzolt objektek listaja
    objects = [*floor, 
               Block(block_size*2,HEGIHT - block_size * 2, block_size), 
               Block(block_size*6,HEGIHT - block_size * 4, block_size),
               Block(block_size*10,HEGIHT - block_size * 2, block_size),
               *spike_sor1]
    #Block(x poz, y poz, block meret)
    
    offset_x = 0
    scroll_area_width = 300
    #block = [Block(0,HEIGHT - block_size,block_size)]a 

    run = True

    while player.hp >0 and run != False: 
        clock.tick(FPS)

        for event in pygame.event.get():
            if event.type==pygame.QUIT:
                run = False
                break

            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE and player.jump_count < 2:
                    player.jump()        
            
        player.loop(FPS)
        handle_move(player,objects)
        draw(window, background, bg_image, player,objects,offset_x)

        if((player.rect.right - offset_x >= WIDTH - scroll_area_width) and player.x_vel > 0) or (
            (player.rect.left - offset_x <= scroll_area_width) and player.x_vel < 0):
            offset_x += player.x_vel
    #halal eseten ezt dobja fel        
    if player.hp <= 0:
        print("meghaltal")
    pygame.quit()
    quit()
    
    
if __name__ == "__main__":
    main(window)