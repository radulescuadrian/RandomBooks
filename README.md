# **Random Books**
Application has 2 Components
- Website
- API - **link everything together**

The application will be developed using Blazor WebAssembly

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
            <li> Credit Cards </li>
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
