#include <string.h>     // for strings
#include <stdio.h>

/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
///////////////////////////////// MAIN //////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////

void main()
{
	char choice = 'x';


	phonebook result;

	cout << "\n                             Phonebook entry process.";
	result.initData();     // calls 'initData();' to prompt user to enter info

	while(choice != 'q')
	{
		cout << "\n\n\n\n\n\n\n\n\n\n\nEnter '1' to view buddy stats, or 'q' to quit: ";
		cin >> choice;
		cout << "\n\n\n\n\n\n\n";
		switch(choice)
		{
		case '1':         // goes here if '1' is entered
			cout << "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nThe results are: \n";
				result.Display();    // calls 'Display();' to display results
				break;

		case 'q':
			cout << "\n Thank you for using PhoneBook v1.0";
			break;                     // quits the program if 'q' is entered
		}
		
	}
	
}
class phonebook         // making a new class 'phonebook'
{
private:
	char name[20];      // these are strings
	char phone[20];     // the numbers between the brackets are the 
	char email[40];     // lengths of chars in one string

public:
	void Display()
	{
	cout << "\n\n Name: " << name;        // the following shows up when
	cout << "\n Phone: " << phone;        // 'Display();' is called and
	cout << "\n E-mail: " << email;       // displays the results
	}

	void initData()      // prompts user to enter information
	{
		cout << "\n\nEnter buddy's name: ";
		cin.getline(name, 20, '\n');     // readstring('name of string',
										 // 'number of chars', 'stop reading')
		
		
		cout << "\nEnter buddy's phone number: ";
		cin.getline(phone, 20, '\n');

		cout << "\nEnter buddy's e-mail: ";
		cin.getline(email, 40, '\n');
	}
}
