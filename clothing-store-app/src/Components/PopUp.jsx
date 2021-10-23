import './PopUp.css'

export default function PopUp({ title, caption, icon, onOk, titleStyle }) {
  return ( 
    <div className="pop-up-wrapper">
      <div className="pop-up-container">
        <label className="title" style={titleStyle}>{ icon }{title}</label>
        <label className="caption">{caption}</label>
        <div className="button-wrapper">
          <button className="default-button" onClick={onOk}>Ok</button>
        </div>
      </div>
      <div className="dimmer"></div>
    </div>
  )
}
