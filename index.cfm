<!DOCTYPE html>
<html lang="en">

<cfquery name="ContactsList" datasource="JDDB" username="JoeyDing" password="dy01_01dy">    
    SELECT * FROM Contacts 
</cfquery>

<cfset countries = ValueList(ContactsList.Country)>

<cfset temp = structNew()>
<cfloop list="#countries#" index="i">
  <cfset temp[i] = "">
</cfloop>
<cfset distinctCountryList = structKeyList(temp)>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Delicious</title>
    <link rel="stylesheet" type="text/css" href="css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">
</head>

<body>
    <section id="menu-list">
        <div class="container">
            <div class="row">
                <div class="col-md-12 text-center marb-35">
                    <h1 class="header-h">Contacts</h1>
                    <p class="header-p">Here you can check all the contacts. </p>
                </div>
                <div class="col-md-12  text-center gallery-trigger">
                    <ul>
                        <li><a class="filter" data-filter="all">Show All</a></li>
                        <cfoutput>
                         <cfloop list="#distinctCountryList#" index="country">
                           <cfset countryToStore = #country#> 
                           <li><a class="filter" data-filter=".#countryToStore#">#country#</a></li>
                            </cfloop>
                         </cfoutput>
                        <li><a href="additem.cfm">Add item</a></li>
                    </ul>
                </div>
                <div id="Container">


                    

                    <!--- 
                    logic to display the corresponding section
                    --->
                    <!--
                    <cfoutput>
                    <cfloop query="ContactsList">
                    <div class="mix menu-restaurant" data-myorder="2">
                                <span class="clearfix">
                                <a class="menu-title" >#ContactsList.PersonName#(#ContactsList.Country#)</a>
                                <span style="left: 166px; right: 44px;" class="menu-line"></span>
                                <span class="menu-price">#ContactsList.Email#</span>
                                </span>
                                <span class="menu-subtitle">#ContactsList.PhoneNumber#</span>
                            </div>
                    </cfloop>
                      </cfoutput>      
                      -->
                        <cfoutput>
                        <cfloop list="#countries#" index="country">
                        <cfset countryToDisplay = #country#> 
                           <div class="mix #countryToDisplay# menu-restaurant" data-myorder="2">
                                <span class="clearfix">
                                <a class="menu-title" >#countries#</a>
                                <span style="left: 166px; right: 44px;" class="menu-line"></span>
                                <span class="menu-price">#countries#</span>
                                </span>
                                <span class="menu-subtitle">#countryToDisplay#</span>
                            </div>
                        </cfloop>
                         </cfoutput>
                </div>
            </div>
        </div>
    </section>


    <script src="js/jquery.min.js"></script>
    <script src="js/jquery.easing.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <script src="js/jquery.mixitup.min.js"></script>
    <script src="js/custom.js"></script>

</body>

</html>