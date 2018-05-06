
class MoneyOverview extends React.Component
{
    constructor(props) {
        super(props);
        this.state = { Total: 0 };

    }

    componentDitMount() {
        
    }

    renderTotal() {
        return <Total key="total0" value={this.state.Total} />
    }

    render() {
        return (
            <div>
                {this.renderTotal()}
            </div>
            );
    }
}

class Total extends React.Component
{
    constructor(props) {
        super(props);
        this.state = { value: this.props.value };
    }
    componentDidMount() {
        var component = this;
        servicehub.server.getMoneyTotal().done(function (data) {
            component.setState({ value: data });
        }).fail(function () {
            console.log("failed getting information from server");
        });
        
    }
    render() {
        return (
            <div className="Total"> 
            <span className="mdl-chip mdl-chip--contact">
                    <span className="mdl-chip__contact mdl-color--teal mdl-color-text--white"><i className="fab fa-monero"></i>A</span>
                <span className="mdl-chip__text">{this.state.value}</span>
            </span>
            </div>

            );
    }
}

// ========================================
var moneyOverviewElement;
function startMoneyOverview() {

    moneyOverviewElement = ReactDOM.render(
        <MoneyOverview />,
        document.getElementById('MoneyOverview')
    );

}

function UpdateMoneyOverview(value) {
    
    moneyOverviewElement.setState({ Total: value });
}