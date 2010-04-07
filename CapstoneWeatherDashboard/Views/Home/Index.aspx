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

            $('#search').children('fieldset').each(function() {
                if ($(this).attr('id') == 'addressSearch') {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            });

            $('#searchby').change(function() {
                var selected = $(this).val()
                $('#search').children('fieldset').each(function() {
                    if ($(this).attr('id') == selected) {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Weather Incident Verification
    </h1>
    <% using (Html.BeginForm("Index", "WeatherIncident", FormMethod.Get, new {id = "searchForm" }))
       { %>
    <div id="dateRange">
        <h2>
            1. Select your date range
        </h2>
        <fieldset>
            <table>
                <tr>
                    <td class="label">
                        <label for="startDate">
                            Start date
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("startDate", "", new { @class = "text placeholder", placeholder = DateTime.Today.AddDays(-7).ToShortDateString() }) %><span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="endDate">
                            End date
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("endDate", "", new { @class = "text placeholder", placeholder = DateTime.Today.ToShortDateString() })%><span class="required">*</span>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    
    <div id="searchRadius">
        <h2>
            2. Pick Your Search Radius
        </h2>
        <fieldset>
            <table>
                <tr>
                    <td class="label">
                        <label for="radius">
                            Radius (miles)
                        </label>
                    </td>
                    <td>
                        <select id="radius" name="radius">
                            <option value="">0</option>
                            <option value="5">5</option>
                            <option value="10">10</option>
                            <option value="15">15</option>
                            <option value="25">25</option>
                            <option value="50">50</option>
                        </select>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="eventTypes">
        <h2>
            3. Filter Event Types
        </h2>
        <fieldset>
            <table>
                <tr>
                    <td class="label">
                        <label for="eventType">
                            Only Show
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
            4. Search
        </h2>
        <table>
            <tr>
                <td class="label">
                    <label for="searchby">Search By: </label>                
                </td>
                <td>
                <select id="searchby" style="margin-bottom: 10px;">
                    <option value="addressSearch">Address</option>
                    <option value="geocodeSearch">Geocode</option>
                    <option value="policySearch">Policy Number</option>
                </select>
                </td>
            </tr>
        </table>
        <fieldset id="addressSearch">
            <table>
                <tr>
                    <td></td>
                    <td><span id="addrError" class="error"></span></td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="address">
                            Address
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("address", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="city">
                            City
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("city", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="state">
                            State
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("state", "", new {@class = "text"}) %>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="zipCode">
                            Zip code
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("zipCode", "", new {@class = "text"}) %><span class="required">*</span>
                    </td>
                </tr>
            </table>
            <p>
                <input id="addressSearchSubmit" type="submit" value="Search Address" class="submit" name="addressSearch" disabled="disabled" />
            </p>
        </fieldset>
        <fieldset id="geocodeSearch">
            <table>
                <tr>
                    <td></td>
                    <td><span id="geoError" class="error"></span></td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="latitude">
                            Latitude
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("latitude", "", new {@class = "text"}) %><span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="longitude">
                            Longitude
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("longitude", "", new {@class = "text"}) %><span class="required">*</span>
                    </td>
                </tr>
            </table>
            <p>
                <input id="geocodeSearchSubmit" type="submit" value="Search Geocode" class="submit" name="geocodeSearch" disabled="disabled" />
            </p>
        </fieldset>
        <fieldset id="policySearch">
            <table>
                <tr>
                    <td></td>
                    <td><span id="policyError" class="error"></span></td>
                </tr>
                <tr>
                    <td class="label">
                        <label for="policyNumber">
                            Policy Number
                        </label>
                    </td>
                    <td>
                        <%= Html.TextBox("policyNumber", "", new {@class = "text"}) %><span class="required">*</span>
                    </td>
                </tr>
                <tr>
                    <td class="label">
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
                <input id="policySearchSubmit" type="submit" value="Search Policy" class="submit" name="policySearch" disabled="disabled" />
            </p>
        </fieldset>
        <span style="color:red; float:right; padding-top:10px;">* - Required Fields</span>
        <br style="clear:both;" />
    </div>
    <% } %>
</asp:Content>
