#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    /* Comment Blocks Example
    asd
    */
    //asd
    
    char *input;
    int a,b,c;
    
    a=7;
    b=12;
    c=a+b;

    //printf - kiiratás
    input = "World!!! \n";
    printf("Hello! \n");
    printf(input);
    printf("**********************\n");
    printf("7 + 12 = ");
    printf("%d\n", 7+12);
    printf("**********************\n");
    printf("%d + %d = ", a, b);
    printf("%d\n", c);
    printf("**********************\n");
    
    //scanf - beolvasás
    printf("Ket szam osszeadasa!\n");
    printf("Irja be az 'a' erteket!\n");
    scanf("%d", &a);
    printf("Irja be a 'b' erteket!\n");
    scanf("%d", &b);
    c = a + b;
    printf("%d + %d = ", a, b);
    printf("%d\n", c);
    printf("**********************\n");    
    

    getchar();
    getchar();
    //system("PAUSE");

    return 0;
}

