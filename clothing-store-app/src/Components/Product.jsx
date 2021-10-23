import React, { useEffect, useState } from 'react'
import { useParams, useRouteMatch, Redirect } from 'react-router-dom'

import { Role } from '../Types/Role'

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faChevronRight } from '@fortawesome/free-solid-svg-icons'
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons'

import './Product.css'

function Product({ productData, expanded, history, authentication, host, onBuy, onCart, onRemove }) {
  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const { user, token, role } = authentication

  const hasRedirectData = history && history.location && history.location.state && history.location.state.redirectData

  if (!productData && !hasRedirectData) {
    throw new Error("No product data received!")
  }

  const [data, setData] = useState(productData ? productData : history.location.state.redirectData)

  const [clicked, setClicked] = useState(false)

  const [currentImageId, setCurrentImageId] = useState(0)

  const [productCount, setProductCount] = useState(1)

  let match = useRouteMatch()

  const images = (data && data.images) ? data.images.map((image, id) => {
    return <img className="image" src={image.url} key={id} alt={data.description} />
  }) : []

  const productClicked = () => {
    if (!expanded) {
      setClicked(true)
    }
  }
  if (clicked) {
    return <Redirect to={{
      pathname: `${match.url}/${data.id}`,
      state: { redirectData: data }
    }} />
  }

  return (
    <div className={`product-container${ expanded ? '' : ' product-container-clickable' }`} onClick={productClicked}>
      { data &&
        <div>
          <div className="images-container">
            <button onClick={() => {
              if (data.images[currentImageId - 1]) {
                setCurrentImageId(currentImageId - 1)
              }
            }} disabled={!data.images[currentImageId - 1]} style={!expanded ? {opacity: 0} : {opacity: 1}}>
              <FontAwesomeIcon icon={faChevronLeft} />
            </button>
            { data.images ? <img src={data.images[currentImageId].url} alt={data.description} /> : <div className="placeholder" />  }
            <button onClick={() => {
              if (data.images[currentImageId + 1]) {
                setCurrentImageId(currentImageId + 1)
              }
            }} disabled={!data.images[currentImageId + 1]} style={!expanded ? {opacity: 0} : {opacity: 1}}>
              <FontAwesomeIcon icon={faChevronRight} />
            </button>
          </div>
          <div className="title-container">
            <div className="title-line">
              <h1>{data.title}</h1>
              <label>{data.price * productCount}zł</label>
            </div>
            <div className="descriptive-line">
              <p className="product-description">{data.description}</p>
              <label className="description">Price</label>
            </div>

            <div className="buy-buttons">
              { (expanded && role === Role.Customer) && <div className="counter">
                <input type="number" min={1} max={data.available} value={productCount} 
                  onChange={ e => {
                    const count = parseInt(e.target.value)
                    if (count >= 1 && count <= 10) {
                      setProductCount(count)
                    }
                  }}/>
                <label>Count</label>
              </div> }
              { role === Role.Customer &&
                <div>
                  <button className="default-button" onClick={() => onBuy(data.id, productCount)}>Buy</button>
                  <button className="invert-button" onClick={() => onCart(data.id, productCount)}>Cart</button>
                </div>
              }
              {
                role === Role.Manager &&
                <div>
                  <button className="invert-button red-invert-button" onClick={() => onRemove(data.id) }>Remove</button>
                </div>
              }
            </div>

            { expanded &&
              <div className="info">
                <div className="available">
                  <label className="value">{data.available}</label>
                  <label className="description">Available</label>
                </div>

                <div className="material">
                  <label className="value">{data.material.title}</label>
                  <label className="description">{data.material.description}</label>
                  <div className="color-info">
                    <div className="swatch" style={{ background: data.material.color.hexCode }}></div>
                    <label className="name" style={{ color: data.material.color.hexCode }}>{data.material.color.name}</label>
                  </div>

                  <div className="materials-raw">
                    { data.material.materials.map((material, id) => {
                      return (
                        <div key={id} className="material-raw">
                          <label className="value">{material.amount}</label>
                          <label className="description">{material.raw.name}</label>
                          <div className="separator"></div>
                        </div>
                      )
                    }) }
                  </div>

                  <div className="size">
                    <label>
                      {data.size.name}
                    </label>
                    <label>Size</label>
                  </div>

                  <div className="type">
                    <label>
                      {data.type.name}
                    </label>
                    <label>Category</label>
                  </div>
                </div>


              </div>
            }
          </div>
         
        </div>
      }
    </div>
  )
}

export default Product
