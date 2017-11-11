<%@ Page Title="Logged" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ExternalLogin.aspx.cs" Inherits="SocialLoginASP.ExternalLogin" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
        <div class="row" style="height:100px;"></div>
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-10 col-sm-12">
                <div class="jumbotron">
                    <asp:ModelErrorMessage ID="ModelErrorMessage3" runat="server" ModelStateKey="Provider" CssClass="field-validation-error" />   
                    <asp:PlaceHolder runat="server" ID="userNameForm">
                        <fieldset>
                            <h2 class="display-3"> <%: ProviderName %> </h2>
                            <p class="lead"> Hello <strong><%: ProviderUserName %></strong>, </p>
                            <hr class="my-4"/>
                            <div class="form-inline">
                                <asp:Image ID="imgSocial" runat="server" CssClass="img-Social" ImageUrl="#" />
                                <p>You've authenticated with <strong><%: ProviderName %></strong> as
                                    <strong><%: ProviderUserName %></strong></p>
                            </div>
                            <hr class="my-4"/>
                            <asp:Button ID="btnBack" CssClass="btn btn-" runat="server" Text="< Back" CausesValidation="false" OnClick="btnBack_Click" />
                        </fieldset>
                    </asp:PlaceHolder>
                </div>
            </div>
        </div>
   </asp:Content>
