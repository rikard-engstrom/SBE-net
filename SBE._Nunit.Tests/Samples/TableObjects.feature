@sample
Feature: Using table as object in scenario

Scenario: Add two numbers
	Given I have entered into the calculator
	| name | email |
	|      |       |
	When I press add
	Then the result should be on the screen
	| name | email |
	|      |       |
