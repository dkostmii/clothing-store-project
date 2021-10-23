import { useState, useEffect, useRef } from 'react'
import { useRouteMatch, Switch, Route, Link, useHistory } from 'react-router-dom'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronRight } from '@fortawesome/free-solid-svg-icons'
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons'

import { Role } from '../Types/Role'

import Product from '../Components/Product'

import { 
  ProductsRequest as Request,
  CartRequests,
  BuyRequest,
  DeleteProduct
} from '../Services/Request'

import './Products.css'


function Products({ authentication, host, onInfo, onError }) {

  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const history = useHistory()

  const addProductRedirect = () => {
    history.push('/Products/Add')
  }

  const defaultLink = `${host}/Products`

  const { user, token, role } = authentication

  const [json, setJson] = useState({})
  
  const [error, setError] = useState()
  const [loaded, setLoaded] = useState(false)
  let fetchCancelled = useRef(false)

  let match = useRouteMatch()

  const onSuccess = result => {
    if (!fetchCancelled.current) {
      setJson(result)
      setLoaded(true)
    }
  }

  const onFail = error => {
    if (!fetchCancelled.current) {
      setError(error)
    }
  }

  const productBuy = (id, count) => {
    BuyRequest(json => {
      onInfo("Buy", "Successfully ordered product")
    }, error => {
      console.error(error)
      onError("Error", error)
    }, host, { id: user.id, token, data: { productId: id, count } })
  }

  const productCart = (id, count) => {
    CartRequests.Post(json => {
      onInfo("Cart", "Your product is added to a cart")
    }, error => {
      console.error(error)
      onError("Error", error)
    }, host, { id: user.id, token, data: { productId: id, count } })
  }

  const productRemove = (id) => {
    DeleteProduct(() => {
      onInfo("Delete product", "Successfully deleted product")
      history.push('/Products')
    }, error => {
      onError("Error", error)
    }, host, { id: user.id, token })
  }

  useEffect(() => {
    fetchCancelled.current = false
    if (!loaded && !error) {
      Request(onSuccess, onFail, host)
    }

    // Prevent state change when comp umounted
    return () => {
      fetchCancelled.current = true
    }
  }, [loaded, error, host])

  const title = loaded ? <h1>Explore from {json.count} products</h1> : <h1>Our products</h1>

  const errorMessage = <p style={{color: '#FF0000'}}>Error loading products: {JSON.stringify(error)}</p>

  const loadingMessage = <p>Loading products...</p>

  const products = json.results ? json.results.map((result, id) => {
    return ( 
      <Product 
        productData={result}
        key={id} 
        host={host} 
        authentication={authentication}
        onBuy={productBuy}
        onCart={productCart}
        onRemove={productRemove}
        /> )
  }) : []

  return (
    <Switch>
      <Route path={`${match.url}/:productId`} render={ (props) => {
        const productProps = {
          host,
          authentication,
          href: defaultLink,
          expanded: true,
          onBuy: productBuy,
          onCart: productCart,
          onRemove: productRemove,
          ...props
        }
        return ( <div className="products-page-container">
          <div className="title-container">
            <Link to={match.url} className="return-link">
              <FontAwesomeIcon icon={faChevronLeft} className="icon" />
              All products
            </Link>
          </div>
          <div className="products">
            <Product {...productProps} />
          </div>
        </div> )
      }} />
      <Route path={match.url}>
        <div className="products-page-container">
          <div className="title-container">
            { title }
            { role === Role.Manager &&
              <button className="default-button" onClick={addProductRedirect}>Add product</button>
            }
          </div>
          { error && errorMessage }
          { !loaded && loadingMessage }
          <div className="products">
            { loaded && products }
          </div>
          <div className="nav-buttons">
            { json.prev && <button className="icon-button" onClick={() => Request(onSuccess, onFail, host, json.prev)}><FontAwesomeIcon className="icon" icon={faChevronLeft} /></button> }
            { json.next && <button className="icon-button" onClick={() => Request(onSuccess, onFail, host, json.next)}><FontAwesomeIcon className="icon" icon={faChevronRight} /></button> }
          </div>
        </div>
      </Route>
    </Switch>
   
  )
}

export default Products
