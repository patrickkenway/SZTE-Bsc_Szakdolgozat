import pygame
from classes import Player_class

pygame.init()
FramePerSec = pygame.time.Clock()
SCREEN_WIDTH = 1200
SCRENN_HEIGHT = 1080
screen = pygame.display.set_mode((SCREEN_WIDTH,SCRENN_HEIGHT))
pygame.display.set_caption("Game")

player_color = (200,120,200)
player_w = 40
player_h=60
player = pygame.Rect((220,220,player_w,player_h))


player_speed = 5 #vel
fps = 60
is_jump = False
jumpCount = 0
jumpMax = 15
fall=0

run = True
while run:
    FramePerSec.tick(fps)
    player.clamp_ip(screen.get_rect())
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False
        if event.type == pygame.KEYDOWN: 
            if not is_jump and event.key == pygame.K_SPACE:
                is_jump = True
                jumpCount = jumpMax    
    #-----------------------movement---------------------------

    keys = pygame.key.get_pressed()
    player.centerx = (player.centerx + (keys[pygame.K_d] - keys[pygame.K_a]) * player_speed) 

    #----------------------jump-----------------------------------
    
    if not (is_jump):
        fall += 0.2
        player.y += fall
        #player = mainleft_img.get_rect(topleft = (x, y))
        if player.bottom > 800:
            player.bottom = 800
            y = player.top
            fall = 0
        if keys[pygame.K_SPACE]:  # jumping code
            isJump = True
            fall = 0
    else:
        if jumpCount >= -10:
            player.y -= (jumpCount * abs(jumpCount)) * 0.5
            jumpCount -= 1
        else:
            jumpMax = 10
            is_jump = False 
    screen.fill((0,0,0))
    pygame.draw.rect(screen,player_color,player)
    #pygame.draw.circle(screen,(255,0,0),player.center,15)
    pygame.display.update()
pygame.quit()