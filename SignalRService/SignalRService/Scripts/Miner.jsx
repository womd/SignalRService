var thisComponent;
var MinerListBox = React.createClass({

    componentDidMount() {
        //   window.componentHandler.upgradeElement(this.root);
       

    },

    getInitialState: function () {

        var data = []

        return { data: data };
    },

    getData: function() {
        setTimeout(() => {

            thisComponent = this;
            servicehub.server.getMinerlistInitialState().done(function (data) {

                thisComponent.setState({ data: data.Miners });


            }).fail(function () {

                alert("failed");

            });

        }, 3000)
    },

    componentDidMount: function() {
        this.getData();
    },

  

    render: function () {
        return (
            React.createElement('div', { className: 'MinerListBox' },
                <MinerList data={this.state.data} />
            )

        );
    }
});


var MinerList = React.createClass({

    componentDidMount() {
       //   window.componentHandler.upgradeElement(this.root);
        //window.componentHandler.upgradeDom('MaterialList', 'mdl-list');
      //  componentHandler.upgradeElement(this.getDOMNode(), "mdl-list");
     

    },

    componentWillUnmount() {
      //  window.componentHandler.downgradeElements(this.root);
    },

    render: function () {

        var miners = this.props.data.map(function (aminer) {
            return (
                <Miner ConnectionId={aminer.ConnectionId} key={aminer.Id} ClientIp={aminer.ClientIp} Hps={aminer.Hps} IsMobile={aminer.IsMobile} IsRunning={aminer.IsRunning} Throttle={aminer.Throttle}></Miner>
               
            );
        });


        return (

            <ul className="mdl-list">
                {miners}
            </ul>
        );

    }
});

var Miner = React.createClass({

    componentDidMount() {
        //   window.componentHandler.upgradeElement(this.root);
      //  componentHandler.upgradeElement(this.getDOMNode(), "mdl-list");
     //   componentHandler.upgradeDom('Miner', 'mdl-list__item');
        componentHandler.upgradeDom();
      
    },

    onChangeThrootle: function (event) {
     //   this.setState({ typed: event.target.value });
    },

    render: function () {
        return (

            
            <li className="mdl-list__item">
                <span className="mdl-list__item-primary-content">
               
                   
                    <span>
                        <span>{this.props.ClientIp}</span>
                        <label className="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="checkbox-1">
                            <input type="checkbox" id="checkbox-1" className="mdl-checkbox__input" defaultChecked={this.props.IsRunning} />
                            <span className="mdl-checkbox__label">IsRunning</span>
                        </label>
                       
                 
                    </span>
                    
             
                    <span class="mdl-badge" data-badge="{this.props.Hps}">Hps {this.props.Hps}</span>
                   

                </span>


                <span className="mdl-list__item-secondary-content">

                   
                  
                    <span>
                        <label className="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="checkbox-ismobile">
                            <input type="checkbox" id="checkbox-ismobile" className="mdl-checkbox__input" defaultChecked={this.props.IsMobile} disabled />
                            <span className="mdl-checkbox__label">IsMobile</span>
                        </label>

                        <span>
                            <span> {this.props.Throttle}</span>
                            <p style={{ width: 180 + 'px' }}>
                               
                                
                                    <input id="sliderThrottle" className="mdl-slider mdl-js-slider" type="range" id="s1" min="0" max="1" value="0.9" step="0.1" onChange="{this.onChangeThrootle}" />
                              
                               
                            </p>
                        </span>
                    </span>
                   
                </span>

            </li>
        );
    }
});


function onChangeThrootle(e) {
    console.log("changeing throotle...");
}

    ReactDOM.render(<MinerListBox />, document.getElementById('reactiveMiners'));


