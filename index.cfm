<!DOCTYPE html>
<html lang="en">

<cfquery name="Contacts" datasource="JDDB" username="JoeyDing" password="dy01_01dy">    
    SELECT * FROM Contacts 
</cfquery>

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
                        <li><a class="filter" data-filter=".category-1"><b>#GetMovies.Style#</b></a></li>
                        <li><a class="filter" data-filter=".category-2">Europe</a></li>
                        <li><a class="filter" data-filter=".category-3">Africa</a></li>
                        <li><a href="additem.cfm">Add item</a></li>
                    </ul>
                </div>
                <div id="Container">
                    <div class="mix category-1 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-1 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-1 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-1 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-2 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-2 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-2 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-2 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-3 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-3 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-3 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
                    <div class="mix category-3 menu-restaurant" data-myorder="2">
                        <span class="clearfix">
                        <a class="menu-title" href="#" >Person Name</a>
                        <span style="left: 166px; right: 44px;" class="menu-line"></span>
                        <span class="menu-price">France</span>
                        </span>
                        <span class="menu-subtitle">Neque porro quisquam est qui dolorem</span>
                    </div>
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