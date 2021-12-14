<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VacationHistory.aspx.cs" Inherits="VacationCalendar.VacationHistory" %>

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

        .invalid {
            border-color: red;
        }
    </style>
    <title>VacationsHistory</title>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <div class="container">
                <div class="row">
                    <div name="entering-dates-block" class="col-xs-6 col-md-4">
                        <h3>Enter vacation dates</h3>
                        <div name="vacation-dates-input" class="border img-rounded" style="background-color: #E6E6E6; padding: 8px 15px">
                            <div class="form-group">
                                <label for="fromInput">Date Start</label>
                                <asp:TextBox id="dateStartInput" runat="server" type="text" placeholder="yyyymmdd" class="form-control datepicker" required="required" autocomplete="off" />
                            </div>
                            <div class="form-group">
                                <label for="toInput">Date End</label>
                                <asp:TextBox id="dateEndInput" runat="server" type="text" placeholder="yyyymmdd" class="form-control datepicker" required="required" autocomplete="off" />
                            </div>
                            <div class="container" style="margin-left: 0; padding-left: 0">
                                <div class="row center-block text-center" style="margin-left: 0; padding-left: 0">
                                    <div class="col-sm-2 col-md-1 text-left" style="margin-left: 0; padding-left: 0">
                                        <asp:Button ID="submitButton" runat="server" Text="Submit" CssClass="btn" OnClick="FormSubmit" style="background-color: #F7F7F7; border-color: #BDBDBD; padding: 5px 15px;  margin-top: 5px; margin-bottom: 5px;" />
                                    </div>
                                    <div id="submitResultMessage" runat="server" class="col-sm-2 img-rounded" style="background-color: white; margin: 9px 20px; padding: 2px 5px" visible="false" >
                                        <asp:Label ID="successMessageLabel" runat="server" style="color: #46A40F;" Visible="false" >Vacation added!</asp:Label>
                                        <asp:Label ID="errorMessageLabel" runat="server" style="color: #CC1616;" Visible="false" >Invalid date entered!</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div name="vacation-history-block" class="col-xs-6 col-md-4">
                        <h3>Vacation history</h3>
                        <asp:GridView ID="vacationHistory" runat="server" AutoGenerateColumns="false" CssClass="table" GridLines="None" Visible="true">
                            <Columns>
                                <asp:BoundField DataField="From"  DataFormatString="{0:yyyyMMdd }" HeaderText="Date from" />
                                <asp:BoundField DataField="To"  DataFormatString="{0:yyyyMMdd }" HeaderText="Date to" />
                                <asp:BoundField DataField="DurationBusinessDays" HeaderText="# of working days" />
                                <asp:BoundField DataField="" HeaderText="" />
                            </Columns>
                        </asp:GridView>
                        <div id="noVacationsMessage" runat="server" class="col-xs-6 col-md-8 img-rounded" style="margin: 70px 10px; padding: 5px" visible="false">
                            <asp:Label runat="server">Отсутствует информация про ранее внесенный отпуск</asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </form>
    <script type="text/javascript">
        const dateStartInput = $('#dateStartInput');
        const dateEndInput = $('#dateEndInput');
        const submitButton = $('#submitButton');

        if (!IsDateValid(dateStartInput.val()) || !IsDateValid(dateEndInput.val())) {
            submitButton.prop('disabled', true);
        }

        $('.datepicker').datepicker({
            format: 'yyyymmdd',
            autoclose: true,
            todayHighlight: true,
            weekStart: 1,
            forceParse: false
        });

        dateStartInput.change(() => {
            DateValidate(dateStartInput);
        });

        dateEndInput.change(() => {
            DateValidate(dateEndInput);
        });

        function DateValidate(item) {
            if (item != null) {
                const [domItem] = item;

                if (!IsDateValid(item.val())) {
                    domItem.classList.add('invalid');
                    submitButton.prop('disabled', true);
                } else {
                    domItem.classList.remove('invalid');

                    if (IsDateValid(dateStartInput.val()) && IsDateValid(dateEndInput.val())) {
                        submitButton.prop('disabled', false);
                    }
                }
            }
        }

        function IsDateValid(date) {
            const dateValid = new RegExp('^[0-9]{4}(0[1-9]|1[012])(0[1-9]|1[0-9]|2[0-9]|3[01])$');

            if (date != null) {
                return dateValid.test(date);
            }
        }       
    </script>
</body>
</html>