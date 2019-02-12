import 'whatwg-fetch';
import { parseJson } from '../utils'
import store from "store";

const NaloziApi = {
  getGlavnaKnjiga: () => {
    return fetch(`/api/glavnaKnjiga`, {
      method: 'GET',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    }).then(parseJson)
  },
  
  submitNalog: (nalog) => {
    return fetch('/api/nalozi', {
      method: nalog.Id ? 'PUT' : 'POST',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(nalog)
    })
  },

  getPromena: (id) => {
    return fetch(`/api/nalozi/promena/${id}`, {
      method: 'GET',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    }).then(parseJson)
  },

  zakljucaj: (id, version) => {
    return fetch(`/api/nalozi/${id}/zakljucaj?version=${version}`, {
      method: 'PUT',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    });
  },

  otkljucaj: (id, version) => {
    return fetch(`/api/nalozi/${id}/otkljucaj?version=${version}`, {
      method: 'PUT',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    });
  },

  obrisi: (id, version) => {
    return fetch(`/api/nalozi/${id}?version=${version}`, {
      method: 'DELETE',
      headers: {
        Authorization : `Bearer ${store.getState().oidc.user.access_token}`
      }
    });
  },
}

export default NaloziApi;