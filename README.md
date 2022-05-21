# **Random Books**
Application has 3 Components
- Website
- Warehouse App
- Courier App
- API - **link everything together**

The application will be developed using Blazor, WPF and Xamarin

----

# Courier App
Only **couriers** can login<br/>
Couriers will take an order from the orders tab taking them to the **order details** page<br/>
There couriers can **start the order**, the navigation being implemented using Google Maps API<br/>
Other details include **calling the customer**<br/>
After delivering the goods the couriers tap **order delivered**<br/>

Couriers can view their **performance** so far, also their **rating** on rated deliveries<br/>
They can also modify their profile by inputing a **picture**<br/>

<h4><b>Orders</b></h4>
<ul>
    <li> View/Take Orders </li>
    <ul>
        <li> Check if the order has been taken already </li>
        <li> If an order has been taken, don't show order list until the started one is finished </li>
        <li> Start/Stop delivery </li>
        <li> Take a break / Resume work </li>
        <ul>
            <li> If the number of breaks exceed 1 hour prevent any more breaks </li>
        </ul>
    </ul>
</ul>
<h4><b>Profile</b></h4>
<ul>
    <li> View performance metrics, rating </li>
    <li> View/Edit certain details </li>
</ul>

----

# Website
Only **customers** and **admins** can login<br/>
If an **employee** logins the message will be user/pass incorrect so the end user cannot discover an employee account<br/>
As an **admin** you can only see the admin dashboard<br/>
**Products** will include the full list of products with filters by categories, authors etc.<br/>
Clicking a category/author in product detail page will open the full list of products containg that<br/>
**Contact** page will provide a way to comunicate with the company and see a google maps location of if<br/>
**Profile** page will only be visible to logged in users<br/>

<h4><b>Pages</b></h4>
<ul>
    <li> Customer </li>
    <ul>
        <li> Home </li>
        <li> Products </li>
        <ul>
            <li>Filters</li>
        </ul>
        <li> About Us </li>
        <li> Contact </li>
        <li> Profile</li>
        <ul>
            <li> Account details </li>
            <li> Addresses </li>
            <ul>
                <li> If an address is deleted and no order contains it then delete it from the DB </li>
            </ul>
            <li> Credit Cards - <i>encrypted with MD5</i></li>
            <li> Orders - <i>rate orders</i> </li>
            <ul>
                <li> Active </li>
                <li> History </li>
            </ul>
        </ul>
    </ul>
    <li> Admin </li>
    <ul>
        <li> View some metrics </li>
        <li> View orders </li>
        <li> View/Add/Edit employees/couriers </li>
        <li> View/Add/Edit products </li>
    </ul>
</ul>
<h4><b>Cart</b></h4>
<ul>
    <li> View cart contents </li>
    <li> Empty cart </li>
    <li> Proceed to checkout </li>
</ul>

----

# Warehouse App
Only **employees** can login
Logout after 10 min of inactivity

<h4><b>Dashboard</b></h4>
<ul>
    <li> Number of available couriers </li>
    <li> Number of available orders </li>
    <li> Orders Completed </li>
</ul>
<h4><b>Orders</b></h4>
<ul>
    <li> View orders </li>
    <li> Take orders </li>
    <ul>
        <li> Check if an order has already been taken </li>
        <li> Only one at a time </li>
        <li> If an order is in progress open it </li>
    </ul>
    <li> Process order </li>
    <ul>
        <li> (prepare books, put them in a box) </li>
        <li> Mark order as ready to ship </li>
    </ul>
    <li> If after 6 hours there are not at least 5 orders ready to ship, send existing ones </li>
</ul>
