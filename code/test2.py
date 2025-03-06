import pygame

pygame.init()

SCREEN_WIDTH = 1200
SCRENN_HEIGHT = 900

screen = pygame.display.set_mode((SCREEN_WIDTH,SCRENN_HEIGHT))

player = pygame.Rect((300,150,50,50))

square1 = pygame.Rect((300, 200, 30, 30))

#-----------------------placeholder---------------------------
#-----------------------placeholder---------------------------

speed = 1
run = True
while run:
    point = pygame.mouse.get_pos()
    collide = player.collidepoint(point)
    collide2 = player.colliderect(square1)
    color=(255,0,0) if collide else (255,255,255)
    screen.fill((0,0,0))
    
    color=(255,0,0) if collide2 else (255,255,255)
    pygame.draw.rect(screen,color,player)
    pygame.draw.rect(screen,(255,0,255),square1)

    player.clamp_ip(screen.get_rect())

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False
#-----------------------movement---------------------------

    key = pygame.key.get_pressed()
    if key[pygame.K_a] == True:
        player.move_ip(-speed,0)
    elif key[pygame.K_d] == True:
        player.move_ip(speed,0)
    elif key[pygame.K_w] == True:
        player.move_ip(0,-speed)
    elif key[pygame.K_s] == True:
        player.move_ip(0,speed)
    
    
    
    pygame.display.update()
pygame.quit()
