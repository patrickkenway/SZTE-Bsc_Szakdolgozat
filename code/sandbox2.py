import pygame

pygame.init()
window = pygame.display.set_mode((1200, 900))
clock = pygame.time.Clock()

player = pygame.sprite.Sprite()
player.image = pygame.Surface((30, 30), pygame.SRCALPHA)
pygame.draw.circle(player.image, (255, 0, 0), (15, 15), 15)
player.rect = player.image.get_rect(center = (150, 235))
all_sprites = pygame.sprite.Group([player])

y, vel_y = player.rect.bottom, 0
vel = 5
ground_y = 250
acceleration = 10
gravity = 0.5

run = True
while run:
    clock.tick(100)
    acc_y = gravity
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False
        if event.type == pygame.KEYDOWN: 
            if vel_y == 0 and event.key == pygame.K_SPACE:
                acc_y = -acceleration

    keys = pygame.key.get_pressed()    
    player.rect.centerx = (player.rect.centerx + (keys[pygame.K_RIGHT] - keys[pygame.K_LEFT]) * vel)
    
    vel_y += acc_y
    y += vel_y
    if y > ground_y:
        y = ground_y
        vel_y = 0
        acc_y = 0
    player.rect.bottom = round(y)

    window.fill((0, 0, 64))
    pygame.draw.rect(window, (64, 64, 64), (0, 250, 300, 100))
    all_sprites.draw(window)
    pygame.display.flip()

pygame.quit()
exit() 