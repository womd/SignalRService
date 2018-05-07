
class LuckyGameWinningRules extends React.Component {
    constructor(props) {
        super(props);
        this.state = { rules : [] };
    }

    componentDidMount() {
        var component = this;
        servicehub.server.getLuckyGameWinningRules(signalRGroup).done(function (data) {
            component.setState({ rules : data });
        }).fail(function () {
            console.log("failed getting luckyGameWinningRules from server");
        });
    }

    renderRule(element) {
        return <WinningRule ID={element.ID} AmountMatchingCards={element.AmountMatchingCards} WinFactor={element.WinFactor} />
    }

    render() {
        return (
            <div>
            {
                this.state.rules.map((element) =>
                        this.renderRule(element))
            }
            </div>
            );
    }

}

class WinningRule extends React.Component {
    constructor(props) {
        super(props);
        this.state = { ruleId : this.props.ID, amountMatchingCards : this.props.AmountMatchingCards, winFactor : this.props.WinFactor };
        
    }
   
    render() {
        return (
            <div>
                <WinningRuleEditItem ruleId={this.state.ruleId} amountMatchingCards={this.state.amountMatchingCards} winFactor={this.state.winFactor} />
            </div>
            );
    }
}

class WinningRuleEditItem extends React.Component {
    constructor(props) {
        super(props);
        this.itemRemoveClicked = this.itemRemoveClicked.bind(this);
        this.state = { ruleId: this.props.ruleId, amountMatchingCards: this.props.amountMatchingCards, winFactor: this.props.winFactor}
    }

    amountMatchingCardsChanged() {
    }

    itemRemoveClicked() {
        var component = this;
        servicehub.server.removeLuckyGameWinningRule().done(function (data) {

        }).fail(function (data) {

        });
    }

    render() {
        return (
            <div className="winningruledititem">
            <input type="hidden" id="wre_id" name="wre_id" value={this.state.ruleId} />
            <div className="mdl-textfield mdl-js-textfield">
                <input onChange={this.amountMatchingCardsChagned} className="mdl-textfield__input" type="text" id="wre_amc" name="wre_amc" pattern="-?[0-9]*(\.[0-9]+)?" value={this.state.amountMatchingCards} />
                <label class="mdl-textfield__label" for="wre_amc">gleiche Symbole</label>
            </div>
            <div className="mdl-textfield mdl-js-textfield">
                <input className="mdl-textfield__input" type="text" id="wrc_wft" name="wre_wft" pattern="-?[0-9]*(\.[0-9]+)?" value={this.state.winFactor} />
                <label class="mdl-textfield__label" for="wrc_wft">Gewinnfaktor</label>
                </div>
                <button className="deletebtn" onClick={this.itemRemoveClicked} >
                    <i title="delete" className="fas fa-minus"></i>
                </button>
            </div>
            );
    }
}

//



// ========================================

var winningRulesElement;
function startLuckyGameWinningRules() {

    winningRulesElement = ReactDOM.render(
        <LuckyGameWinningRules key="lgwr0" />,
        document.getElementById('LuckyGameWinningRules')
    );

}