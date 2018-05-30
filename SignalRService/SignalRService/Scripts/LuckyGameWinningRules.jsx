
class LuckyGameWinningRules extends React.Component {
    constructor(props) {
        super(props);
        this.ruleItemAddClicked = this.ruleItemAddClicked.bind(this);
        this.updateList = this.updateList.bind(this);
        this.handleItemUnmount = this.handleItemUnmount.bind(this);
        this.state = { rules : [] };
    }

    componentDidMount() {
        this.updateList();
    }

    updateList() {
        var component = this;
        servicehub.server.getLuckyGameWinningRules(signalRGroup).done(function (data) {
            component.setState({ rules: data });
         
        }).fail(function () {
            console.log("failed getting luckyGameWinningRules from server");
        });
    }

    handleItemUnmount() {
        this.updateList();
    }

    renderRule(element) {
        return <WinningRule key={element.Id} Id={element.Id} AmountMatchingCards={element.AmountMatchingCards} WinFactor={element.WinFactor} unMountMe={this.handleItemUnmount} />
    }

    ruleItemAddClicked() {
        var rules = this.state.rules;
        rules.push({ Id: 0, AmountMatchingCards: 0, WinFactor: 0 });
        this.setState({ rules: rules });
    }

    render() {
        return (
            <div>
                <button className="addbtn" onClick={this.ruleItemAddClicked} >
                    <i title="add" className="fas fa-plus"></i>
                </button>
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
        this.state = { ruleId: this.props.Id, amountMatchingCards: this.props.AmountMatchingCards, winFactor: this.props.WinFactor, unMountMe: this.props.unMountMe };
        
    }
   
    render() {
        return (
            <div>
                <WinningRuleEditItem ruleId={this.state.ruleId} amountMatchingCards={this.state.amountMatchingCards} winFactor={this.state.winFactor} unMountMe={this.state.unMountMe} />
            </div>
            );
    }
}

class WinningRuleEditItem extends React.Component {
    constructor(props) {
        super(props);
        this.itemRemoveClicked = this.itemRemoveClicked.bind(this);
        this.itemSaveClicked = this.itemSaveClicked.bind(this);
        this.state = { ruleId: this.props.ruleId, amountMatchingCards: this.props.amountMatchingCards, winFactor: this.props.winFactor, unMountMe: this.props.unMountMe}
    }

    amountMatchingCardsChanged() {
    }

    itemRemoveClicked() {
        if (this.props.ruleId == 0) {
            this.state.unMountMe();
        }
        else {
            servicehub.server.removeLuckyGameWinningRule(this.props.ruleId, signalRGroup).done(function (data) {

                console.log("ruleitem deleted");
            }).fail(function () {
                console.log("failed request deleting rule");
            });
        }
    }

    itemSaveClicked(e) {


        var itemid = "#wre_id_" + this.state.ruleId;
        var matchamountelementid = "#wre_amc" + this.state.ruleId;
        var winfactorelementid = "#wrc_wft" + this.state.ruleId;

        var item = { Id: this.props.ruleId, AmountMatchingCards: parseInt( $(matchamountelementid).val()), WinFactor: parseFloat( $(winfactorelementid).val()) };

        servicehub.server.addOrUpdateLuckyGameWinningRule(item, signalRGroup).done(function (data) {
            if (!data.Success) {
                alert(data.ErrorMessage);
            }

        }).fail(function () {
            console.log("failed request adding rule....");
        });

    }

    render() {

        var cssClass = "winningruledititem_normal";
        if (this.state.ruleId == 0) {
            cssClass = "winningruledititem_new";
        }

        var itemid = "wre_id_" + this.state.ruleId;
        var matchamountelementid = "wre_amc" + this.state.ruleId;
        var winfactorelementid = "wrc_wft" + this.state.ruleId;


        return (
            <div className={cssClass}>
            <input type="hidden" id={itemid} name="wre_id" value={this.state.ruleId} />
            <div className="mdl-textfield mdl-js-textfield">
                    <input className="mdl-textfield__input" type="text" id={matchamountelementid} name="wre_amc" pattern="-?[0-9]*(\.[0-9]+)?" defaultValue={this.state.amountMatchingCards} />
                <label className="mdl-textfield__label" htmlFor="wre_amc">gleiche Symbole</label>
            </div>
            <div className="mdl-textfield mdl-js-textfield">
                    <input className="mdl-textfield__input" type="text" id={winfactorelementid} name="wre_wft" pattern="-?[0-9]*(\.[0-9]+)?" defaultValue={this.state.winFactor} />
                <label className="mdl-textfield__label" htmlFor="wrc_wft">Gewinnfaktor</label>
                </div>
                <button className="deletebtn" onClick={this.itemRemoveClicked} >
                    <i title="löschen" className="fas fa-minus"></i>
                </button>
                <button className="savebtn" onClick={this.itemSaveClicked} >
                    <i title="speichern" className="fas fa-gavel"></i>
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