import pygame

pygame.init()

SCREEN_WIDTH = 1200
SCRENN_HEIGHT = 900

screen = pygame.display.set_mode((SCREEN_WIDTH,SCRENN_HEIGHT))

player = pygame.Rect((300,250,50,50))

#-----------------------placeholder---------------------------
#-----------------------placeholder---------------------------


run = True
while run:
    point = pygame.mouse.get_pos()
    collide = player.collidepoint(point)
    color=(255,0,0) if collide else (255,255,255)
    screen.fill((0,0,0))
    pygame.draw.rect(screen,color,player)

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False
#-----------------------movement---------------------------
    key = pygame.key.get_pressed()
    if key[pygame.K_a] == True:
        player.move_ip(-1,0)
    elif key[pygame.K_d] == True:
        player.move_ip(1,0)
    elif key[pygame.K_w] == True:
        player.move_ip(0,-1)
    elif key[pygame.K_s] == True:
        player.move_ip(0,1)
    
    
    
    pygame.display.update()
pygame.quit()
