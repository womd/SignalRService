class MoneyOverviewRoom extends React.Component {
    constructor(props) {
        super(props);
        this.state = { Total: 0 };

    }

    componentDidMount() {
        var component = this;
        servicehub.server.getMoneyRoomTotal(signalRGroup).done(function (data) {
            component.setState({ Total: data });
        }).fail(function () {
            console.log("failed getting information from server");
        });
    }

    renderTotal() {
        return <TotalRoom key="roomtotal0" value={this.state.Total} />
    }

    render() {
        return (
            <div>
                {this.renderTotal()}
            </div>
        );
    }
}


class MoneyOverviewUser extends React.Component
{
    constructor(props) {
        super(props);
        this.state = { Total: 0 };

    }

    componentDidMount() {
         var component = this;
        servicehub.server.getMoneyUserTotal().done(function (data) {
            component.setState({ Total: data });
        }).fail(function () {
            console.log("failed getting information from server");
        });
    }

    renderTotal() {
        return <TotalUser key="total0" value={this.state.Total} />
    }

    render() {
        return (
            <div>
                {this.renderTotal()}
            </div>
            );
    }
}

class TotalRoom extends React.Component
{
    constructor(props) {
        super(props);
      
    }
    componentDidMount() {
        //var component = this;
        //servicehub.server.getMoneyTotal().done(function (data) {
        //    component.setState({ value: data });
        //}).fail(function () {
        //    console.log("failed getting information from server");
        //});
        
    }
    render() {
        return (
            <div className="Total"> 
            <span className="mdl-chip mdl-chip--contact">
                    <span className="mdl-chip__contact mdl-color--teal mdl-color-text--white"><i className="fas fa-object-group"></i> </span>
                <span className="mdl-chip__text">{this.props.value}</span>
            </span>
            </div>

            );
    }
}

class TotalUser extends React.Component {
    constructor(props) {
        super(props);

    }
    componentDidMount() {
        //var component = this;
        //servicehub.server.getMoneyTotal().done(function (data) {
        //    component.setState({ value: data });
        //}).fail(function () {
        //    console.log("failed getting information from server");
        //});

    }
    render() {
        return (
            <div className="Total">
                <span className="mdl-chip mdl-chip--contact">
                    <span className="mdl-chip__contact mdl-color--teal mdl-color-text--white"><i className="fas fa-user-astronaut"></i> </span>
                    <span className="mdl-chip__text">{this.props.value}</span>
                </span>
            </div>

        );
    }
}

// ========================================
var moneyOverviewUserElement, moneyOverviewRoomElement;
function startMoneyOverview() {

    moneyOverviewUserElement = ReactDOM.render(
        <MoneyOverviewUser />,
        document.getElementById('MoneyOverviewUser')
    );

    moneyOverviewRoomElement = ReactDOM.render(
        <MoneyOverviewRoom />,
        document.getElementById('MoneyOverviewRoom')
    );

}

function UpdateMoneyOverview(value) {
    
    moneyOverviewUserElement.setState({ Total: value });
}

function UpdateAvailableMoneyOverview(value) {

    moneyOverviewRoomElement.setState({ Total: value });
}