<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Auto-Owners Incident Verification System
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script src="/demo/Scripts/NewSearch.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Weather Incident Verification
    </h1>
    <% using (Html.BeginForm("Index", "WeatherIncident", FormMethod.Get))
       { %>
    <div id="dateRange">
        <h2>
            1. Select your date range
        </h2>
        <fieldset>
            <table>
                <tr>
                    <td>
                        <label for="startDate">
                            Start date
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("startDate", "", new { @class = "text", placeholder = DateTime.Today.AddDays(-7).ToShortDateString() }) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="endDate">
                            End date
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("endDate", "", new { @class = "text", placeholder = DateTime.Today.ToShortDateString() })%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="search">
        <h2>
            2. Search
        </h2>
        <fieldset id="addressSearch">
            <legend>by address</legend>
            <table>
                <tr>
                    <td>
                        <label for="address">
                            Address
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("address", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="city">
                            City
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("city", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="state">
                            State
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("state", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="zipCode">
                            Zip code
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("zipCode", "", new {@class = "text"}) %>
                    </td>
                </tr>
            </table>
            <p>
                <input type="submit" value="Search Address" class="submit" />
            </p>
        </fieldset>
        <fieldset id="geocodeSearch">
            <legend>by geocode</legend>
            <table>
                <tr>
                    <td>
                        <label for="latitude">
                            Latitude
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("latitude", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="longitude">
                            Longitude
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("longitude", "", new {@class = "text"}) %>
                    </td>
                </tr>
            </table>
            <p>
                <input type="submit" value="Search Geocode" class="submit">
            </p>
        </fieldset>
        <fieldset id="policySearch">
            <legend>by policy number</legend>
            <table>
                <tr>
                    <td>
                        <label for="policyNumber">
                            Policy Number
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("policyNumber", "", new {@class = "text"}) %>
                    </td>
                </tr>
            </table>
            <p>
                <input type="submit" value="Search Policy" class="submit" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
