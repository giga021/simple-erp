import React, { Component } from 'react';
import KarticaKonta from './KarticaKonta';
import KarticaKontaApi from './KarticaKontaApi';
import KontaApi from 'Common/KontaApi';

class KarticaKontaContainer extends Component {

  constructor(props) {
    super(props);
    this.reloadData = this.reloadData.bind(this);
    this.setKonto = this.setKonto.bind(this);
    this.state = {
      kartica: [],
      konta: [],
      idKonto: null
    };
  }

  componentWillMount() {
    KontaApi.getKonta()
      .then(konta => this.setState({
        konta: konta || []
      }));
  }

  reloadData() {
    if (this.state.idKonto) {
      KarticaKontaApi.getKarticaKonta(this.state.idKonto)
        .then(kartica => this.setState({ kartica: kartica }));
    }
  }

  setKonto(idKonto) {
    this.setState({ idKonto });
  }

  render() {
    return (
      <KarticaKonta kartica={this.state.kartica}
        konta={this.state.konta}
        setKonto={this.setKonto}
        reloadData={this.reloadData}/>
    );
  }
}

export default KarticaKontaContainer;
