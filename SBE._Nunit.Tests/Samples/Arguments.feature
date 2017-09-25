Feature: Name your arguments
	In order to produce readable documentation scenarios with arguments should be renderd with nice names

Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
