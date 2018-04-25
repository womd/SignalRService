var thisComponent;
var MinerListBox = React.createClass({

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

    render: function () {

        var miners = this.props.data.map(function (aminer) {
            return (
                <Miner ConnectionId={aminer.ConnectionId} key={aminer.Id} ClientIp={aminer.ClientIp} Hps={aminer.Hps} IsMobile={aminer.IsMobile} IsRunning={aminer.IsRunning}></Miner>
               
            );
        });


        return (

            <div className="MinerList">
                {miners}
            </div>
        );

    }
});

var Miner = React.createClass({

    render: function () {
        return (
            <div className="Miner">
                <div className="ConnectionId">
                    {this.props.ConnectionId}
                </div>
                <div className="ClientIp">
                    {this.props.ClientIp}
                </div>
                <div className="IsRemote">
                    {this.props.IsRemote}
                </div>
                <div className="IsRunning">
                    {this.props.IsRunning}
                </div>
            </div>
        );
    }
});






    ReactDOM.render(<MinerListBox />, document.getElementById('reactiveMiners'));


