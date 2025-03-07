import pygame
from pygame.locals import *
pygame.init()

white = 255,255,255
black = 0,0,0
green = 0, 255, 0
blue = 0,0,255

screen_width = 650
screen_height = 800
screen = pygame.display.set_mode([screen_width,screen_height])
pygame.display.set_caption("Darsh's Game")
font = pygame.font.SysFont('Comic Sans MS', 50, 70)

running = 6
speed = 2
fall = 0
x = 25
y = 669
isJump = False
jumpCount = 10
left = False
right = True

def draw_text(text, font, text_col, x, y):
    img = font.render(text, True, text_col)
    screen.blit(img, (x, y))

def draw_rect():
    rect1 = pygame.Rect(0, 0, 25, 800)
    pygame.draw.rect(screen, black, rect1)
    rect2 = pygame.Rect(625, 0, 25, 800)
    pygame.draw.rect(screen, black, rect2)
    rect3 = pygame.Rect(0, 775, 650, 25)
    pygame.draw.rect(screen, green, rect3)

def direction(left, right):
    if left == True:
        screen.blit(mainleft_img, (x, y))
    elif right == True:
        screen.blit(mainright_img, (x, y))
    draw_text("George is dumb", font, green, 100, 400)

mainright_img = pygame.image.load('newgameimg/mainright.png')
mainleft_img = pygame.image.load('newgameimg/mainleft.png')

clock = pygame.time.Clock()
run = True
while run:
    clock.tick(60)
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False

    keys = pygame.key.get_pressed()
    if keys[pygame.K_a] and x > speed - 16:
        x -= speed
        left = True
        right = False
    if keys[pygame.K_d] and x < screen_width - speed - 89:
        x += speed
        left = False
        right = True
    if keys[pygame.K_ESCAPE]:
        pygame.quit()

    if not (isJump):
        fall += 0.2
        y += fall
        player_rect = mainleft_img.get_rect(topleft = (x, y))
        if player_rect.bottom > 775:
            player_rect.bottom = 775
            y = player_rect.top
            fall = 0
        if keys[pygame.K_SPACE]:  # jumping code
            isJump = True
            fall = 0
    else:
        if jumpCount >= -10:
            y -= (jumpCount * abs(jumpCount)) * 0.5
            jumpCount -= 1
        else:
            jumpCount = 10
            isJump = False  # jumping code

    screen.fill(white)
    draw_rect()
    direction(left, right)    
    pygame.display.update()