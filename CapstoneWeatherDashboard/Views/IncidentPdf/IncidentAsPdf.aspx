<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<h1>Weather Incident Report</h1>
<table>
    <tr>
        <td><b>Date: </b><%=ViewData["date"] %></td>
    </tr>
    <tr>
        <td><b>Event Type: </b><%=ViewData["event"] %></td>
    </tr>
    <tr>
        <td><b>Source Url: </b><%=ViewData["url"] %></td>
    </tr>
    <tr>
        <td><b>Map:</b></td>
    </tr>
    <tr>
        <td><img src="http://maps.google.com/maps/api/staticmap?zoom=8&size=512x512&maptype=roadmap&sensor=false&center=<%=ViewData["restOfMapUrl"] %>" /></td>
    </tr>
    <tr><td><b>Key:</b></td></tr>
    <tr>
        <td><b>H</b> - Location Searched For</td>
        <td><b>O</b> - Location Observed At</td>
    </tr>
</table>