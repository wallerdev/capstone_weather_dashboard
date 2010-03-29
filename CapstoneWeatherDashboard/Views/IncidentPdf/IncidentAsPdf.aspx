<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<h1>Weather Incident Report</h1>
<p><b>Date: </b><%=ViewData["date"] %></p>
<p><b>Event Type: </b><%=ViewData["event"] %></p>
<p><b>Source Url: </b><%=ViewData["url"] %></p>