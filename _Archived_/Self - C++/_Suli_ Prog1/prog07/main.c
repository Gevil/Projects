#include <stdio.h>
#include <stdlib.h>
#define ARRAYSIZE 5 // definiált konstans...

int main(int argc, char *argv[])
{
    int tomb[ARRAYSIZE];
    int i;
    
    for(i = 0; i < ARRAYSIZE; i++)
    {
        printf("Kerem a %d. elemet!\n", i+1);
        scanf("%d", & tomb[i]);
    }
    
    for(i = 0; i < ARRAYSIZE; i++)
    {
        printf("%d. elem: %d\n", i+1,tomb[i]);
    }
    
    //min max keresgélés
    int min, max;
    min = tomb[0];
    max = tomb[0];
    for(i = 0; i < ARRAYSIZE; i++)
    {
          if(tomb[i] > max)
          {
              max = tomb[i];
          }
          else if(tomb[i] < min)
          {
              min = tomb[i];
          } 
    }
    printf("\nMaximum: %d\n", max);
    printf("Minimum: %d\n", min);
    
    //buborékrendezés
    int temp;
    int j;
    for(j = 0; j < ARRAYSIZE-1; j++)
    {
          for(i = 0; i < (ARRAYSIZE-1)-j; i++)
          {
                if(tomb[i] < tomb[i+1])
                {
                     temp = tomb[i];
                     tomb[i] = tomb[i+1];
                     tomb[i+1] = temp;
                }
          }
    }
    
    printf("\nRendezett tomb!\n");
    for(i = 0; i < ARRAYSIZE; i++)
    {
        printf("%d. elem: %d\n", i+1,tomb[i]);
    }
    
    getchar();
    getchar();
    return 0;
}
