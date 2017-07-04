<!DOCTYPE html>
<html lang="en">

<cffunction name="saveContacts" >
    <cfargument name="name" type="string" required="yes">
    <cfargument name="country" type="string" required="yes">
    <cfargument name="email" type="string" required="yes">
    <cfargument name="phonenumber" type="string" required="yes">

    <cfquery datasource="JDDB" username="JoeyDing" password="dy01_01dy">
            INSERT INTO Contacts (PersonName,Country,Email,PhoneNumber)
        VALUES     ('#name#','#Country#','#Email#','#PhoneNumber#')
    </cfquery>
    </cffunction>



<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Delicious</title>
    <link rel="stylesheet" type="text/css" href="css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/style.css">
</head>

<body>
    <section id="contact">
        <div class="container">
            <div class="row">
                <div class="col-md-12 text-center">
                    <h1 class="header-h">Add Contact</h1>
                    <p class="header-p">Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy
                        <br>nibh euismod tincidunt ut laoreet dolore magna aliquam. </p>
                    <div class="col-md-12  text-center gallery-trigger">
                        <ul>
                            <li><a href="index.cfm">Home</a></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="row msg-row">
                <div class="col-md-4 col-sm-4 mr-15">
                    <div class="media-2">
                        <div class="media-left">
                            <div class="contact-phone bg-1 text-center"><span class="phone-in-talk fa fa-phone"></span></div>
                        </div>
                        <div class="media-body">
                            <h4 class="dark-blue regular">Phone Numbers</h4>
                            <p class="light-blue regular alt-p">+440 875369208 - <span class="contacts-sp">Phone Booking</span></p>
                        </div>
                    </div>
                    <div class="media-2">
                        <div class="media-left">
                            <div class="contact-email bg-14 text-center"><span class="hour-icon fa fa-clock-o"></span></div>
                        </div>
                        <div class="media-body">
                            <h4 class="dark-blue regular">Opening Hours</h4>
                            <p class="light-blue regular alt-p"> Monday to Friday 09.00 - 24:00</p>
                            <p class="light-blue regular alt-p">
                                Friday and Sunday 08:00 - 03.00
                            </p>
                        </div>
                    </div>
                </div>
                <div class="col-md-8 col-sm-8">
                <cfoutput> 
                    <form action="" method="post" role="form" class="contactForm">
                        <div id="sendmessage">Your booking request has been sent. Thank you!</div>
                        <div id="errormessage"></div>
                        <div class="col-md-6 col-sm-6 contact-form pad-form">
                            <div class="form-group label-floating is-empty">
                           
                                <input type="text" name="name" class="form-control" id="name" placeholder="Your Name" data-rule="minlen:4" data-msg="Please enter at least 4 chars"
                                />
                                <div class="validation"></div>
                           
                            </div>

                        </div>
<div class="col-md-6 col-sm-6 contact-form">
                            <div class="form-group">
                           
                                <input type="text" class="form-control label-floating is-empty" name="country" id="country" placeholder="Country" data-rule="required"
                                    data-msg="This field is required" />
                                <div class="validation"></div>
                           
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 contact-form pad-form">
                            <div class="form-group">
                                <input type="email" class="form-control label-floating is-empty" name="email" id="email" placeholder="Your Email" data-rule="email"
                                    data-msg="Please enter a valid email" />
                                <div class="validation"></div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 contact-form">
                            <div class="form-group">
                           
                                <input type="text" class="form-control label-floating is-empty" name="phone" id="phone" placeholder="Phone Number" data-rule="required"
                                    data-msg="This field is required" />
                                <div class="validation"></div>
                           
                            </div>
                        </div>
                        <div class="col-md-12 contact-form">
                            <div class="form-group label-floating is-empty">
                           
                                <textarea class="form-control" name="message" rows="5" rows="3" data-rule="required" data-msg="Please write something for us"
                                    placeholder="Message"></textarea>
                                <div class="validation"></div>
                           
                            </div>
                        </div>
        <div class="col-md-12 btnpad">
                        <!---
                            <div class="contacts-btn-pad">
                                <button class="contacts-btn" onclick="updateContacts()">Save contact</button>
                            </div>
                        --->
                            <cfform name="New">
                                <cfinput type="submit" value="Save contact" name="save"><br>
                            </cfform> 
                            <cfif isDefined("FORM.save") >
                            <cfoutput>                                
                                Upadated: #saveContacts('#name#','#country#','#email#','#phone#')#
                            </cfoutput>
                            </cfif>
                        </div>
                    </form>
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