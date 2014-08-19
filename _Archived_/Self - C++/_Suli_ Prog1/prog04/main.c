#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
    int a;
begin:
    printf("Irja be egy honap sorszamat!\n");
    scanf("%d", &a);
    
    /*if(a <= 12 && a >=1)
    {*/
        switch(a)
        {
            case 1:
                 printf("januar");
                 break;
            case 2:
                 printf("februar");
                 break;
            case 3:
                 printf("marcius");
                 break;
            case 4:
                 printf("aprilis");
                 break;             
            case 5:
                 printf("majus");
                 break;
            case 6:
                 printf("junius");
                 break;               
            case 7:
                 printf("julius");
                 break;       
            case 8:
                 printf("augusztus");
                 break;       
            case 9:
                 printf("szeptember");
                 break;       
            case 10:
                 printf("oktober");
                 break;       
            case 11:
                 printf("november");
                 break;       
            case 12:
                 printf("december");
                 break;
            default:
                 printf("'a'-nak 1 es 12 kozott kell lennie!\n");
                 printf("*************************************\n");
                 goto begin;
        }
/*    }
    else
    {
        printf("'a'-nak 1 es 12 kozott kell lennie!\n");
        printf("*************************************\n");
        goto begin;
    }
*/    
    getchar();
    getchar();
    return 0;
}
