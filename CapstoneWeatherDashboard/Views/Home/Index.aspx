<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Auto-Owners Incident Verification System
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/demo/Content/NewSearch.css" rel="stylesheet" type="text/javascript" />
    <link href="/demo/Content/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="/demo/Scripts/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script src="/demo/Scripts/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>
    <link href="/demo/Content/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />
    <script src="/demo/Scripts/NewSearch.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('#policyHolderName').autocomplete('<%= Url.Action("Index","PolicyHolderNames") %>', {
                cacheLength: 10,
                minChars: 3,
                matchCase: false,
                matchContains: true,
                scroll: false,
                dataType: 'json',
                parse: function(data) {
                    var rows = new Array();
                    for (var i = 0; i < data.length; i++) {
                        rows[i] = { data: data[i], value: data[i].PolicyHolderName, result: data[i].PolicyHolderName };
                    }
                    return rows;
                },
                formatItem: function(row, i, n) {
                    return row.PolicyHolderName + ' - ' + row.PolicyNumber;
                },
                max: 50
            }).result(function(event, item) {
                $('#policyNumber').val(item.PolicyNumber);
                $('#PolicyStreetAddress').val(item.PolicyHomeAddress.StreetAddress);
                $('#PolicyCity').val(item.PolicyHomeAddress.City);
                $('#PolicyState').val(item.PolicyHomeAddress.State.Name);
                $('#PolicyZipCode').val(item.PolicyHomeAddress.ZipCode);
                $('#PolicyCounty').val(item.PolicyHomeAddress.County);
            });
        });
    </script>
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
                        <%= Html.TextBox("startDate", "", new { @class = "text placeholder", placeholder = DateTime.Today.AddDays(-7).ToShortDateString() }) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label for="endDate">
                            End date
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("endDate", "", new { @class = "text placeholder", placeholder = DateTime.Today.ToShortDateString() })%>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="filter">
        <h2>
            2. Filter Event Types (optional)
        </h2>
        <fieldset>
            <table>
                <tr>
                    <td>
                        <label for="eventType">
                            Event Type to Search For
                        </label>
                    </td>
                    <td>
                        <%=Html.DropDownList("incidentTypes", (IEnumerable<SelectListItem>)ViewData["IncidentTypes"], "All") %>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="search">
        <h2>
            3. Search
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
                <input type="submit" value="Search Address" class="submit" name="addressSearch" disabled="disabled" />
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
                <input type="submit" value="Search Geocode" class="submit" name="geocodeSearch" disabled="disabled" />
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
                <tr>
                    <td>
                        <label for="policyHolderName">
                            Policy Holder Name
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("policyHolderName", "", new { @class = "text" })%>
                    </td>
                </tr>
            </table>
            <span>
                <%= Html.Hidden("PolicyStreetAddress") %>
                <%= Html.Hidden("PolicyCity")%>
                <%= Html.Hidden("PolicyState")%>
                <%= Html.Hidden("PolicyZipCode")%>
                <%= Html.Hidden("PolicyCounty")%>
            </span>
            <p>
                <input type="submit" value="Search Policy" class="submit" name="policySearch" disabled="disabled" />
            </p>
        </fieldset>
    </div>
    <% } %>
</asp:Content>
