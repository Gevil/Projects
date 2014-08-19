#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int a, b;
    printf("Irjon be az 'a' erteket!\n");
    scanf("%d", &a);
    printf("Irjon be a 'b' erteket!\n");
    scanf("%d", &b);
    
    
    if(a < b)
    {
         printf("%d kisebb mint %d\n", a,b);
    }
    else if ( a > b)
    {
        printf("%d nagyobb mint %d\n", a,b);
    }
    else if ( a == b)
    {
        printf("%d egyenlo %d -el\n", a,b);
    }
    else
    {
        printf("valami elromlott :P");
    }

    getchar();
    getchar();
    return 0;
}
