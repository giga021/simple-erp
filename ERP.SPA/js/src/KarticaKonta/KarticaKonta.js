import React, { Component } from 'react';
import ReactTable from 'react-table';
import 'react-table/react-table.css';
import KarticaKontaApi from './KarticaKontaApi';
import KontaApi from 'Common/KontaApi';
import { Button, Input, InputGroup } from 'reactstrap';
import { FaSearch } from 'react-icons/fa';

class KarticaKonta extends Component {

  render() {
    const cellStyle = {
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center'
    }
    const iznosCellStyle = {
      display: 'flex',
      flexDirection: 'column',
      justifyContent: 'center',
      textAlign: 'right'
    }

    const columns = [{
      Header: 'Datum naloga',
      accessor: 'DatumNaloga',
      style: cellStyle,
      Cell: props => <span>{new Date(props.value).toLocaleDateString('sr')}</span>
    }, {
      Header: 'Tip naloga',
      accessor: 'TipNaloga',
      style: cellStyle
    }, {
      Header: 'Datum knjiženja',
      accessor: 'DatumKnjizenja',
      style: cellStyle,
      Cell: props => <span>{new Date(props.value).toLocaleDateString('sr')}</span>
    }, {
      Header: 'Konto',
      accessor: 'Konto',
      style: cellStyle
    }, {
      Header: 'Opis',
      accessor: 'Opis',
      style: cellStyle
    }, {
      Header: 'Duguje',
      accessor: 'Duguje',
      style: iznosCellStyle,
      Cell: props => <span>{props.value.toLocaleString('sr',
        { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
    }, {
      Header: 'Potražuje',
      accessor: 'Potrazuje',
      style: iznosCellStyle,
      Cell: props => <span>{props.value.toLocaleString('sr',
        { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
    }, {
      Header: 'Saldo kumulativno',
      accessor: 'SaldoKumulativno',
      style: iznosCellStyle,
      Cell: props => <span>{props.value.toLocaleString('sr',
        { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
    }]

    return (
      <div>
        <h3 className='page-header'>Kartica konta</h3>
        <InputGroup style={{width:300}}>
          <Input
            onChange={(e) => this.props.setKonto(e.target.value)}
            type='select'>
            <option></option>
            {this.props.konta.map(t =>
              <option key={t.Id} value={t.Id}>{t.Sifra} {t.Naziv}</option>)}
          </Input>
          <Button onClick={this.props.reloadData}><FaSearch/>Pregled</Button>
        </InputGroup>
        <ReactTable
          data={this.props.kartica}
          columns={columns}
        />
      </div>
    );
  }
}

export default KarticaKonta;
