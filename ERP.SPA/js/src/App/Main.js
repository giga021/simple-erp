import React from 'react'
import { Switch, Route } from 'react-router-dom';
import Home from 'Home';
import GlavnaKnjiga from '../GlavnaKnjiga'
import KarticaKonta from '../KarticaKonta'
import CallbackPage from '../CallbackPage'
import { IconContext } from "react-icons";
import { ToastContainer } from 'react-toastify';

const Main = () => (
  <IconContext.Provider value={{ className: "icon" }}>
    <main>
      <Switch>
        <Route exact path='/' component={Home} />
        <Route path='/glavna-knjiga' component={GlavnaKnjiga} />
        <Route exact path='/kartica-konta' component={KarticaKonta} />
      </Switch>
      <ToastContainer />
    </main>
  </IconContext.Provider>
)

export default Main