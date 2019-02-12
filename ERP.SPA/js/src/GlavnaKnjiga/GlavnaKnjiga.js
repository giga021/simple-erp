import React, { Component } from 'react';
import ReactTable from 'react-table';
import 'react-table/react-table.css';
import { Link } from 'react-router-dom'
import { Button } from 'reactstrap'
import { FaLockOpen, FaLock, FaEdit, FaPlus, FaTrashAlt } from 'react-icons/fa';

class GlavnaKnjiga extends Component {

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
      accessor: 'Datum',
      style: cellStyle,
      Cell: props => <span>{new Date(props.value).toLocaleDateString('sr')}</span>
    }, {
      Header: 'Tip',
      accessor: 'TipNaziv',
      style: cellStyle
    }, {
      Header: 'Opis',
      accessor: 'Opis',
      style: cellStyle
    }, {
      Header: 'Broj stavki',
      accessor: 'BrojStavki',
      style: cellStyle
    }, {
      Header: 'Ukupno duguje',
      accessor: 'UkupnoDuguje',
      style: iznosCellStyle,
      Cell: props => <span>{props.value.toLocaleString('sr',
        { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
    }, {
      Header: 'Ukupno potražuje',
      accessor: 'UkupnoPotrazuje',
      style: iznosCellStyle,
      Cell: props => <span>{props.value.toLocaleString('sr',
        { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
    }, {
      id: 'Zakljucan',
      accessor: 'Zakljucan',
      style: cellStyle,
      Cell: props => props.value ?
        <Button onClick={() => this.props.otkljucajNalog(props.original.Id, props.original.Version)}><FaLockOpen />Otključaj</Button> :
        <Button onClick={() => this.props.zakljucajNalog(props.original.Id, props.original.Version)}><FaLock />Zaključaj</Button>
    }, {
      id: 'Promeni',
      accessor: 'Id',
      style: cellStyle,
      Cell: props => <Link to={`${this.props.url}/${props.value}`}>
        <Button disabled={props.original.Zakljucan}><FaEdit />Promeni</Button>
      </Link>
    }, {
      id: 'Obrisi',
      accessor: 'Id',
      style: cellStyle,
      Cell: props => 
        <Button color="danger" disabled={props.original.Zakljucan}
          onClick={() => this.props.obrisiNalog(props.original.Id, props.original.Version)}><FaTrashAlt />Obriši</Button>
    }]

    return (
      <div>
        <h3 className='page-header'>Glavna knjiga</h3>
        <Link to={`${this.props.url}/novi-nalog`}>
          <Button color='success'><FaPlus />Novi nalog</Button>
        </Link>
        <ReactTable
          data={this.props.nalozi}
          columns={columns}
        />
      </div>
    );
  }
}

export default GlavnaKnjiga;