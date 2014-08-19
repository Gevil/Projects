// asd.cpp : Defines the entry point for the console application.
//
#include "stdafx.h"
#include <iostream>
#include <string>
#include <stdio.h>
#include <fstream>
using std::cout;
using std::cin;
using std::ifstream;
using std::ofstream;



class phonebook         // making a new class 'phonebook'
{
public:
	char name[20];      // these are strings
	char phone[20];     // the numbers between the brackets are the 
	char email[40];     // lengths of chars in one string


	void Display()
	{
	cout << "\n\n Name: " << name;        // the following shows up when
	cout << "\n Phone: " << phone;        // 'Display();' is called and
	cout << "\n E-mail: " << email;       // displays the results
	}

	void List()
	{
		FILE *inputfile = fopen("input.dat", "r");
		char c;
		while(c=fscanf(inputfile,"%c") != EOF)
		{
			for(int i=0; i++; i==4)
			{
			}
		}
			
	}

	void initData()      // prompts user to enter information
	{
		int i = 0;
		char c;
		printf("\n\nNev: ");
		while(i<=20)
		{
			if(scanf("%c", &c)=="\n")

			scanf("%c", &name);
			i++;
		}

		printf(name);
		
		
		printf("\nEnter buddy's phone number: ");
		cin.getline(phone, 20, '\n');

		printf("\nEnter buddy's e-mail: ");
		cin.getline(email, 40, '\n');
	}

	void WriteToFile()
	{
		  FILE *fp;
		  char s[80];
		  int t;

		  if((fp=fopen("input.dat", "a")) == NULL) {
			  printf("Cannot open file.\n");
			  exit(1);
		  }
		  


	}
};

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
///////////////////////////////// MAIN //////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////


int _tmain(int argc, _TCHAR* argv[])
{
	char c = 'x';

	phonebook result;
	phonebook pBook[100];

	//FILE *inputfile = fopen("input.dat", "a");

	printf("\n                             Phonebook entry process.");
	
	while(c != 'k')
	{
		printf("\n\n\n\n\n\n\n\n\n Ird be az 'l' betut az adatok listazasahoz.\n");
		printf(" Ird be az 'u' betut uj adatok hozzaadasahoz.\n");
		printf(" Ird be a 'w' betut az adatok mentesehez.\n");
		printf(" Ird be az 'k' betut a kilepeshez.\n");
		scanf("%c", &c);
		printf("\n\n\n\n\n\n\n");
		switch(c)
		{
		case '1':
			printf("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nThe results are: \n");
				result.Display();
				break;

		case 'k':
			printf("\n Vege!");
			break;
		
		case 'w':
			result.WriteToFile();
			break;

		case 'u':
			result.initData();
			break;
		}
		
	}
}

