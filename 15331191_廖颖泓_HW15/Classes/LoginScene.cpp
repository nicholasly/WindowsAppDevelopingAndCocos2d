#include "LoginScene.h"
#include "cocostudio/CocoStudio.h"
#include "json/rapidjson.h"
#include "json/document.h"
#include "json/writer.h"
#include "json/stringbuffer.h"
#include "Global.h"
#include "GameScene.h"
#include <regex>
#define database UserDefault::getInstance()
using std::to_string;
using std::regex;
using std::match_results;
using std::regex_match;
using std::cmatch;
using namespace rapidjson;
USING_NS_CC;

using namespace cocostudio::timeline;

#include "json/document.h"
#include "json/writer.h"
#include "json/stringbuffer.h"
using namespace  rapidjson;

Scene* LoginScene::createScene()
{
    // 'scene' is an autorelease object
    auto scene = Scene::create();
    
    // 'layer' is an autorelease object
    auto layer = LoginScene::create();

    // add layer as a child to scene
    scene->addChild(layer);

    // return the scene
    return scene;
}





// on "init" you need to initialize your instance
bool LoginScene::init()
{
    // 1. super init first
    if ( !Layer::init() )
    {
        return false;
    }

    Size size = Director::getInstance()->getVisibleSize();
    visibleHeight = size.height;
    visibleWidth = size.width;

    textField = TextField::create("Player Name", "Arial", 30);
    textField->setPosition(Size(visibleWidth / 2, visibleHeight / 4 * 3));
    this->addChild(textField, 2);

    auto button = Button::create();
    button->setTitleText("Login");
    button->setTitleFontSize(30);
    button->setPosition(Size(visibleWidth / 2, visibleHeight / 2));
	button->addClickEventListener(CC_CALLBACK_1(LoginScene::login_button_click, this));
    this->addChild(button, 2);

    return true;
}

void LoginScene::login_button_click(Ref * pSender) {
	HttpRequest *request = new HttpRequest();
	request->setRequestType(HttpRequest::Type::POST);
	request->setUrl("http://localhost:8080/login");
	request->setResponseCallback(CC_CALLBACK_2(LoginScene::onHttpRequestCompletedLogin, this));

	string username = "username=" + textField->getString();
	const char *postData = username.c_str();
	request->setRequestData(postData, strlen(postData));

	cocos2d::network::HttpClient::getInstance()->send(request);
	request->release();
}

void LoginScene::onHttpRequestCompletedLogin(HttpClient * sender, HttpResponse * response) {
	if (!response) {
		return;
	}
	if (!response->isSucceed()) {
		log("response failed");
		log("error buffer: %s", response->getErrorBuffer());
		return;
	}

	string buffer = Global::toString(response->getResponseData());
	string header = Global::toString(response->getResponseHeader());

	rapidjson::Document d;
	d.Parse<0>(buffer.c_str());
	if (d.HasParseError()) {
		CCLOG("GetParseError%s\n", d.GetParseError());
	}

	if (d.IsObject() && d.HasMember("result") && d["result"].GetBool()) {
		Global::gameSessionId = Global::getSessionIdFromHeader(header);
		database->setStringForKey("sessionid", Global::gameSessionId);
		database->setStringForKey("header", header);
		database->setStringForKey("body", buffer);

		auto scene = GameScene::createScene();
		Director::getInstance()->replaceScene(scene);
	}
}