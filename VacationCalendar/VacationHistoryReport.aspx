<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VacationHistoryReport.aspx.cs" Inherits="VacationCalendar.VacationHistoryReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.7/css/bootstrap.css" rel="stylesheet" type="text/css"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:300" rel="stylesheet" />
    <style type="text/css">
        body {
            font-family: 'Roboto', sans-serif;
        }
        .result-success {
            color: #46A40F;
        }
        .result-error {
            color: #CC1616;
        }
    </style>
    <title>VacationHistoryReport</title>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <div class="container">
                <div class="row" style="margin-top: 50px; margin-bottom: 20px">
                    <div class="col-md-3">
                        <asp:Button ID="importVacationsButton" runat="server" CssClass="btn" style="background-color: #F0F0F0; border-color: #BDBDBD" OnClick="ImportVacations" />
                    </div>
                    <div class="col-md-3" style="margin-left: 20px; margin-top: 7px">
                        <asp:Label ID="importResultMessageLabel" runat="server" Visible="false" ></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div name="vacation-history-block" class="col-xs-6 col-md-6">
                        <h3>Vacation history</h3>
                        <asp:GridView ID="vacationHistoryReport" runat="server" AutoGenerateColumns="false" CssClass="table" GridLines="None" Visible="true">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="User Id" />
                                <asp:BoundField DataField="Name" HeaderText="User name" />
                                <asp:BoundField DataField="From"  DataFormatString="{0:yyyyMMdd }" HeaderText="Date from" />
                                <asp:BoundField DataField="To"  DataFormatString="{0:yyyyMMdd }" HeaderText="Date to" />
                                <asp:BoundField DataField="DurationBusinessDays" HeaderText="# of working days" />
                            </Columns>
                        </asp:GridView>
                        <div id="noVacationsMessage" runat="server" class="col-xs-6 col-md-8 img-rounded" style="margin: 70px 10px; padding: 5px" visible="false">
                            <asp:Label runat="server">Отсутствует информация про ранее внесенные отпуска пользователей</asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </form>    
</body>
</html>
