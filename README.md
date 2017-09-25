# Specification by example (SBE) 
is a collaborative approach to defining requirements and business-oriented functional tests for software products.
It's based on capturing and illustrating requirements using realistic examples instead of abstract statements.

https://en.wikipedia.org/wiki/Specification_by_example

# Uses SpecFlow from http://specflow.org/
1. Install Visual Studio plugin for Specflow
	* Open Extensions and Updates
	* Search for Specflow
	* Install Specflow for Visual Studio 2017
2.	Add nuget package for Specflow
3.  Add nuget package for Specflow and your test framework of choice (Specflow.NUnit)

# Key concepts
## Assembly
All tests are grouped into assemblies. An assembly contains multiple epics.

## Epic
Group a set of features by assigning them a tag like @epic:My_epic_feature_set
Features without epic-tags will be grouped into an epic with blank as name.

## Feature
Name and description for a group of scenarios.

## Scenario
A test scenario for a feature