<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<h1>
    Weather Incident Report</h1>
<table>
    <% for (int i = 0; i < (int)ViewData["size"]; i++)
       { %>
    <tr>
        <td>
            <b>Date: </b>
            <%= ((string[])ViewData["dates"])[i] %>
        </td>
    </tr>
    <tr>
        <td>
            <b>Event Type: </b>
            <%= ((string[])ViewData["events"])[i] %>
        </td>
    </tr>
    <tr>
        <td>
            <b>Source Url: </b>
            <%= ((string[])ViewData["urls"])[i] %>
        </td>
    </tr>
    <tr>
        <td>
            <b>Map:</b>
        </td>
    </tr>
    <tr>
        <td>
            <img src="http://maps.google.com/maps/api/staticmap?zoom=8&size=512x512&maptype=roadmap&sensor=false&center=<%= ((string[])ViewData["restOfMapUrls"])[i] %>" />
        </td>
    </tr>
    <tr>
        <td>
            <b>Key:</b>
        </td>
    </tr>
    <tr>
        <td>
            <b>H</b> - Location Searched For
        </td>
        <td>
            <b>O</b> - Location Observed At
        </td>
    </tr>
    <% } %>
</table>
