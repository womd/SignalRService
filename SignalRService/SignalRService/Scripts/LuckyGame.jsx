﻿var cardCollection = [];

window.findReactComponent = function (el) {
    for (const key in el) {
        if (key.startsWith('__reactInternalInstance$')) {
            const fiberNode = el[key];

            return fiberNode && fiberNode.return && fiberNode.return.stateNode;
        }
    }
    return null;
};

function get_random(list) {
    var rand = Math.floor((Math.random() * list.length));
    return list[rand];
}

function findWithAttr(array, attr, value) {
    for (var i = 0; i < array.length; i += 1) {
        if (array[i][attr] === value) {
            return i;
        }
    }
    return -1;
}

function getNextCard(list, currentKey) {
    var idx = findWithAttr(list, 'Key', currentKey);
    if (idx == list.length - 1) {
        idx = 0;
    }
    idx++;
    return list[idx];
}

function getFirstCard(list) {
    return list[0];
}

var slotsSpinning = true;
class LuckyGame extends React.Component {
    constructor(props) {
        super(props);
        
        this.play = this.play.bind(this);
        this.renderSlotBoard = this.renderSlotBoard.bind(this);
        this.renderResMessage = this.renderResMessage.bind(this);
        this.amountSliderChanged = this.amountSliderChanged.bind(this);
        this.state = {
            slotCount: this.props.slotCount, resultData: { Cards: {}, UserTotalAmount: 0 } };
    }
    componentDidMount() {
       
    }

    renderResMessage() {
        return <ResMessage key="rmx0" resultData={this.state.resultData} />;
    }
    renderSlotBoard() {
        return <SlotBoard key="sb01" slotCount={this.state.slotCount} resultData={this.state.resultData} />
    }
    renderSlider() {
        return <input id="slideramount" className="mdl-slider mdl-js-slider" type="range"
            min="1" max="100000" defaultValue="10" tabIndex="0" onChange={this.amountSliderChanged} /> 
    }

    play() {
        
        var component = this;
        var amount = $('#amount').val();
        if(amount)
        servicehub.server.getLuckyGameResult(this.props.slotCount, signalRGroup, amount).done(function (data) {

            slotsSpinning = false;
            component.resultData = data;
            setTimeout(function () {

                component.setState({ slotCount: component.props.slotCount, resultData: data });

                setTimeout(function () {
                    slotsSpinning = true;
                }, 2000)


            },150);
            
           

        }).fail(function () {
            console.log("failed getting luckyGameRes from server");
        });
    }

    amountSliderChanged() {
        $('#amount').val($('#slideramount').val());
        componentHandler.upgradeDom();
    }

    render() {

        return (
            <div>
                {this.renderSlotBoard()}
               
                <form>
                <div className="mdl-textfield mdl-js-textfield">
                    <input className="mdl-textfield__input" type="text" pattern="[0-9]*" id="amount" />
                        <label className="mdl-textfield__label" htmlFor="amount">Betrag</label>
                        <span className="mdl-textfield__error">Bitte nur ganze Zahlen!</span>
                </div>
                    <button type="button" onClick={this.play} className="mdl-button mdl-js-button mdl-button--raised mdl-button--colored">
                        Go
                </button>
                </form>
                {this.renderSlider()}
              
                
               
               
                {this.renderResMessage()}
               

            </div>);
    }
}


class ResMessage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        var cssclass = "undef";
        var statmsg = ""
        var msgsymbol = "";
        var amountval = 0;

        if (this.props.resultData.AmountWon > 0) {
            statmsg = "";
            cssclass = "won";
            amountval = this.props.resultData.AmountWon
            msgsymbol = "+";
        }
        else {
            if (this.props.resultData.Message) {
                statmsg = this.props.resultData.Message;
                cssclass = "error";
            }
            else {
                if (this.props.resultData.AmountLost) {
                    statmsg = "";
                    cssclass = "lost";
                    amountval = this.props.resultData.AmountLost
                    msgsymbol = "-";
                }
            }
        }
       
        return (
            <div>
                <div className="mdl-card mdl-shadow--2dp">
                    <div className="mdl-card__supporting-text">
                        <div className={cssclass}>{statmsg}
                            <div className="msg_wrap">
                                <div className="msg_symbol">{msgsymbol}  </div>
                                <div className="msg_amount">{amountval} </div>
                            </div>
                        </div>
                    </div>
                    <div className="mdl-card__actions mdl-card--border">
                        <a className="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect">
                            Teilen
                        </a>
                    </div>
                </div>
            </div>
            );
    }
}


class SlotBoard extends React.Component {
    constructor(props) {
        super(props);
        this.buildSlots = this.buildSlots.bind(this);
        this.state = { slotCount: this.props.slotCount, cardData: this.props.resultData }
    }
    buildSlots(slotCount) {
        var slots = [];
        var cnt = 0;
        for (i = 1; i <= slotCount; i++) {
            var resdata = null;
            if (this.props.resultData.Cards[cnt]) {
                resdata = this.props.resultData.Cards[cnt];
            }
            else {
               resdata = getFirstCard(cardCollection);
            }
            slots.push({ key: "slot_" + i, spinStartDelay: i * 300, cardData: resdata});
            cnt++;
        }
        return slots;
    }
    renderSlot(element) {
        return <CardSlot key={element.key} delay={element.spinStartDelay} cardData={element.cardData}/>;
    }
    render() {
        return (
            <div>
                {
                    this.buildSlots(this.state.slotCount).map((aslot) =>
                        this.renderSlot(aslot))
                }
            </div>
        );
    }
}


class CardSlot extends React.Component {
    constructor(props) {
        super(props);
        this.tick = this.tick.bind(this);
        this.spinStart = this.spinStart.bind(this);
        //console.log("here at init: " + this.props.delay);
     
            this.state = { card: this.props.cardData };
     
    }
    componentDidMount() {
         setTimeout(this.spinStart, this.props.delay);
       
    }
    tick() {
        if (slotsSpinning) {
            this.setState({ card: getNextCard(cardCollection, this.state.card.Key), spinning:true });
        }
    }
    spinStart() {
        this.interval = setInterval(this.tick, 150);
    }
    spinStop() {
        clearInterval(this.interval);
    }

    render() {
        var symbol = "";
        if (this.state.spinning) {
            symbol = this.state.card.Symbol;
        }
        else {
            if (this.props.cardData && this.props.cardData.Key) {
                symbol = this.props.cardData.Symbol;
            }
        }
        return (
            <div className="cardslot">
                <i className={symbol}></i>
            </div>
        );
    }
}



function play() {
    console.log("play...");
}

//



// ========================================
function startLuckyGame() {

 gameElement =  ReactDOM.render(
        <LuckyGame slotCount={6} />,
        document.getElementById('LuckyGame')
    );
   
}


