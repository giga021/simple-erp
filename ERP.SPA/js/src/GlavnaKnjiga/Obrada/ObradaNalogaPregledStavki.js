import React, { Component } from 'react';
import {
  Button, Table
} from 'reactstrap';
import { FaStop, FaMinus } from 'react-icons/fa';


class ObradaNalogaPregledStavki extends Component {

  render() {
    return (
      <div>
        <h6>Stavke</h6>
        <Table hover size="sm" className='stavke-table'>
          <thead>
            <tr>
              <th>Konto</th>
              <th className='right'>Duguje</th>
              <th className='right'>Potražuje</th>
              <th className='right' style={{width: 110}}></th>
              <th className='right' style={{width: 100}}></th>
            </tr>
          </thead>
          <tbody>
            {this.props.stavke.map(s => {
              let className = '';
              if (s.Stornirana)
                className = 'stornirana';

              return <tr key={s.Id} className={className}>
                <td>{s.Konto}</td>
                <td className='right'>{s.Duguje.toLocaleString('sr',
                  { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className='right'>{s.Potrazuje.toLocaleString('sr',
                  { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className='right'>
                  <Button color="warning"
                    disabled={s.Stornirana}
                    onClick={() => this.props.stornirajStavku(s.Id)}>
                    <FaStop />Storniraj
                    </Button>
                </td>
                <td className='right'>
                  <Button color="danger"
                    disabled={s.Stornirana}
                    onClick={() => this.props.ukloniStavku(s.Id)}>
                    <FaMinus />Ukloni
                    </Button>
                </td>
              </tr>
            })}
          </tbody>
        </Table>
      </div>
    );
  }
}

export default ObradaNalogaPregledStavki;
