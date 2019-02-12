import React from 'react'
import {
  Collapse,
  Navbar,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
} from 'reactstrap';
import { Link } from 'react-router-dom'
import userManager from 'userManager';

const Header = () => (
  <Navbar color="primary" dark expand="md">
    <NavbarBrand tag={Link} to="/">Početna</NavbarBrand>
    <Collapse isOpen={true} navbar>
      <Nav className="ml-auto" navbar>
        <NavItem><NavLink tag={Link} to="/glavna-knjiga">Glavna knjiga</NavLink></NavItem>
        <NavItem><NavLink tag={Link} to="/kartica-konta">Kartica konta</NavLink></NavItem>
        <NavItem>
          <NavLink href="#" onClick={() => {
            userManager.createSignoutRequest().then(x => {
              userManager.getUser().then(u => {
                userManager.removeUser();
                window.location = `${x.url}&id_token_hint=${u.id_token}`
                //fetch(`${x.url}&id_token_hint=${u.id_token}`);
              })
            });
          }}>Odjava</NavLink>
        </NavItem>
      </Nav>
    </Collapse>
  </Navbar>
)

export default Header
