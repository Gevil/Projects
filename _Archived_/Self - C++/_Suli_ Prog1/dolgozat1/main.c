#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int a, b, c;
    printf("Haromszog szerkeszthetosege.\n");
    printf("Irja be az 'a' oldal erteket!\n");
    scanf("%d", &a);
    printf("Irja be a 'b' erteket!\n");
    scanf("%d", &b);
    printf("Irja be a 'c' erteket!\n");
    scanf("%d", &c);
    
    if((a + b > c) && (a + c > b) && (c + b > a))
    {
          printf("Mindharom oldalra teljesul a feltetel.\n A haromszog szerkesztheto.");
          printf("    .    \n");
          printf("   / /\   \n");
          printf("  /   /\  \n");
          printf(" /_____/\ \n");
    }
    else
    {
        printf("A haromszog nem szerkesztheto!");
    }

    getchar();
    getchar();    
    return 0;
}
