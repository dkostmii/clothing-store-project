import React, { useState, useEffect } from 'react'
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  Redirect
} from 'react-router-dom'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faTshirt } from '@fortawesome/free-solid-svg-icons'
import { faSignOutAlt } from '@fortawesome/free-solid-svg-icons'
import { faCog } from '@fortawesome/free-solid-svg-icons'
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons'
import { faExclamationCircle } from '@fortawesome/free-solid-svg-icons'

import './App.css'

import Products from './Pages/Products'
import SignIn from './Pages/SignIn'
import SignUp from './Pages/SignUp'

import MainPage from './Pages/MainPage'
import NotFound from './Pages/NotFound'

import { Role } from './Types/Role'

import ShippingInfoPage from './Pages/Customer/ShippingInfoPage'
import OrdersPage from './Pages/Customer/OrdersPage'
import CartPage from './Pages/Customer/CartPage'

import SettingsPage from './Pages/SettingsPage'

import PopUp from './Components/PopUp'

import AddProductPage from './Pages/AddProductPage'

import AuthStorage from './Services/AuthStorage'
import { CustomerBalanceRequest as BalanceRequest } from './Services/Request'

const emptyAuth = {
  role: null,
  token: null,
  user: null
}

function isEmpty({ role, token, user }) {
  return !role && !token && !user
}

export default function App() {
  const [authentication, setAuthentication] = useState(emptyAuth)

  const [state, setState] = useState()

  const [isAuth, setIsAuth] = useState(false)

  const [popUpMessage, setPopUpMessage] = useState()

  const host = "https://localhost:44352"

  const createInfoMessage = (title, message) => {
    setPopUpMessage({
      title,
      message,
      type: 'info'
    })
  }

  const createErrorMessage = (title, message) => {
    setPopUpMessage({
      title,
      message,
      type: 'error'
    })
  }

  const closeMessage = () => setPopUpMessage()

  // if user sign in or up
  const auth = ({ role, token, user }) => {
    AuthStorage.save({ role, token, user })

    setAuthentication({
      role,
      token,
      user
    })
    setIsAuth(true)
  }

  // if user settings modified
  const userChanged = (data) => {
    setAuthentication(prevAuthentication => {
      const newAuthentication = { ...prevAuthentication, ...{ user: data } }
      AuthStorage.save(newAuthentication)

      return newAuthentication
    })
  }

  const logout = () => {
    AuthStorage.remove(authentication)

    setIsAuth(false)
    setAuthentication(emptyAuth)
  }

  // load credentials on first render
  useEffect(() => {
    const credentials = AuthStorage.load()
    if (credentials) {
      setAuthentication(credentials)
      setIsAuth(true)
    }
  }, [])

  useEffect(() => {
    if (authentication.user && authentication.role === Role.Customer) {
      BalanceRequest(json => {
        if (json.balance) {
          setState({ balance: json.balance })
        }
      }, error => {
        createErrorMessage("Error", "Error occured when loading customer balance: " + error)
      }, host, { id: authentication.user.id, token: authentication.token })
    }
  }, [authentication.token, authentication.user, authentication.role])


  return (
    <Router>
      { popUpMessage && <PopUp
        title={popUpMessage.title} 
        caption={popUpMessage.message}
        icon={<FontAwesomeIcon icon={faInfoCircle} style={popUpMessage.type === 'info' ? { color: '#20a3d6' } : {}}/> }
        titleStyle={popUpMessage.type === 'error' ? { color: '#d9372b' } : {}}
        onOk={closeMessage}
        /> }
      <div className="header">
        <div className="nav-links">
          <Link to="/" className="main-page-link"><FontAwesomeIcon icon={faTshirt} /> Clothing store</Link>
          <Link to="/Products">Products</Link>
        </div>
        { 
          isAuth ? (
            <div className="profile-links">
              <label>Hello, {authentication.user.firstName}! { authentication.role === Role.Customer && `Your balance ${state && state.balance ? state.balance : 0}`}</label>
              { authentication.role === Role.Customer && 
                ( <div className="customer-links">
                    <Link to="/Cart">Cart</Link>
                    <Link to="/Orders">Orders</Link>
                    <Link to="/ShippingInfo">Shipping Info</Link>
                  </div> ) 
              }
              <Link to="/Settings">
                <FontAwesomeIcon icon={faCog} style={{fontSize: '1.6em'}} />
              </Link>
              <button className="logout-button" onClick={logout}>
                <FontAwesomeIcon icon={faSignOutAlt} style={{fontSize: '1.6em'}} />
              </button>
            </div> )
            : 
            (
              <div className="auth-links">
                <Link to="/SignIn">Sign In</Link>
                <Link to="/SignUp">Sign Up</Link>
              </div>
            )
        }
      </div>
      <div className="app-wrapper">
        <Switch>
          <Route exact path="/">
            <MainPage />
          </Route>
          <Route exact path="/SignIn">
            { isAuth ? <Redirect to="/" /> : <SignIn onSignIn={auth} host={host} /> }
          </Route>

          <Route path="/SignUp">
            { isAuth ? <Redirect to="/" /> : <SignUp onSignUp={auth} host={host} /> }
          </Route>

          <Route path="/Cart">
            <CartPage host={host} authentication={authentication} />
          </Route>

          <Route path="/Orders">
            <OrdersPage host={host} authentication={authentication} />
          </Route>

          <Route path="/Products/Add">
            <AddProductPage host={host} authentication={authentication} onInfo={createInfoMessage} onError={createErrorMessage} />
          </Route>

          <Route path="/Products">
            <Products host={host} authentication={authentication} onInfo={createInfoMessage} onError={createErrorMessage} />
          </Route>

          <Route path="/ShippingInfo">
            { isAuth ? <ShippingInfoPage authentication={authentication} host={host} /> : <Redirect to="/SignIn" /> }
          </Route>

          <Route path="/Settings">
            { isAuth ? <SettingsPage authentication={authentication} onUserChanged={userChanged} host={host} /> : <Redirect to="/SignIn" /> }
          </Route>

          <Route path="*">
            <NotFound>
              <Link to="/">Return to Main Page</Link>
            </NotFound>
          </Route>

        </Switch>
      </div>
    </Router>
  )
}

