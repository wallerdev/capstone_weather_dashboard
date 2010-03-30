<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<h1>Weather Incident Report</h1>
<p><b>Date: </b><%=ViewData["date"] %></p>
<p><b>Event Type: </b><%=ViewData["event"] %></p>
<p><b>Source Url: </b><%=ViewData["url"] %></p>
<p><b>Map: </b></p>
<br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
<p><img src="http://maps.google.com/maps/api/staticmap?zoom=10&size=512x512&maptype=roadmap&sensor=false&center=<%=ViewData["restOfMapUrl"] %>" /></p>
<p><b>Key:</b></p>
<p><b>H</b> - Location Searched For</p>
<p><b>O</b> - Location Observed At</p>