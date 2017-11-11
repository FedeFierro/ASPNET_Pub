<%@ Page Title="Social Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SocialLoginASP.Default" %>

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
            <div class="col-md-6 col-sm-12">
                <div class="jumbotron">
                    <h1 class="display-3"> Sample ASP.NET: </h1>
                    <p class="lead"> External Login with Social Networks</p>
                    <hr class="my-4"/>
                    <p>Using DotNetOpenAuth, Fix Facebook Client And Add Instragam Client</p>
                </div>
            </div>
            <div class="col-md-1"></div>
            <div class="col-md-4 col-sm-12">
                <div>
                    <asp:ListView runat="server" ID="providerDetails" ItemType="Microsoft.AspNet.Membership.OpenAuth.ProviderDetails"
                                SelectMethod="GetProviderNames" ViewStateMode="Disabled">
                        <ItemTemplate>
                            <button type="submit" name="provider" class="btn btn-social  btn-lg btn-<%#: Item.ProviderName %>" value="<%#: Item.ProviderName %>"
                                    title="Log in using your <%#: Item.ProviderDisplayName %> account.">
                                    <%#: Item.ProviderDisplayName %>
                            </button>
                            &nbsp
                            <br />
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <p>No external authentication services configured.Go to AuthConfig.cs </p>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
</asp:Content>
