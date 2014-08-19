#include <stdio.h>
#include <stdlib.h>

int main(int argc, char *argv[])
{
	double koord1, koord2;
	int benn=0, osszes=0;
	do{
		printf("Adja meg az egyik koordinatat:");
		scanf("%lf", &koord1);
		printf("Adja meg a masik koordinatat:");
		scanf("%lf", &koord2);	
		if (((koord1+koord2)*(koord1+koord2)) <= 1)
		{
			benn++;
			osszes++;
			printf("%d", osszes);
		}
		else {
			osszes++;
			printf("%d", osszes);
			}
		}
	while((koord1 != 0) && (koord2 != 0));
	printf("Az megadattok koordinatak %d szazaleka esik az egyseg koron belulre.\n", (osszes%benn));
    getchar();
    getchar();
    return 0;
}
