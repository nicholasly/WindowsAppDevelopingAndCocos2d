#include "GameScene.h"
#include "json/rapidjson.h"
#include "json/document.h"
#include "json/writer.h"
#include "json/stringbuffer.h"
#include <regex>
#define database UserDefault::getInstance()
using std::regex;
using std::match_results;
using std::regex_match;
using std::cmatch;
using namespace rapidjson;

USING_NS_CC;

cocos2d::Scene* GameScene::createScene() {
    // 'scene' is an autorelease object
    auto scene = Scene::create();

    // 'layer' is an autorelease object
    auto layer = GameScene::create();

    // add layer as a child to scene
    scene->addChild(layer);

    // return the scene
    return scene;
}

bool GameScene::init() {
    if (!Layer::init())
    {
        return false;
    }

    Size size = Director::getInstance()->getVisibleSize();
    visibleHeight = size.height;
    visibleWidth = size.width;

    score_field = TextField::create("Score", "Arial", 30);
    score_field->setPosition(Size(visibleWidth / 4, visibleHeight / 4 * 3));
    this->addChild(score_field, 2);

    submit_button = Button::create();
    submit_button->setTitleText("Submit");
    submit_button->setTitleFontSize(30);
    submit_button->setPosition(Size(visibleWidth / 4, visibleHeight / 4));
	submit_button->addClickEventListener(CC_CALLBACK_1(GameScene::submit_button_click, this));
    this->addChild(submit_button, 2);

    rank_field = TextField::create("", "Arial", 30);
    rank_field->setPosition(Size(visibleWidth / 4 * 3, visibleHeight / 4 * 3));
    this->addChild(rank_field, 2);

    rank_button = Button::create();
    rank_button->setTitleText("Rank");
    rank_button->setTitleFontSize(30);
    rank_button->setPosition(Size(visibleWidth / 4 * 3, visibleHeight / 4));
	rank_button->addClickEventListener(CC_CALLBACK_1(GameScene::rank_button_click, this));
    this->addChild(rank_button, 2);

	header_field = TextField::create("", "Arial", 20);
	header_field->setPosition(Size(visibleWidth / 2, visibleHeight / 8 * 4));
	this->addChild(header_field, 2);

	body_field = TextField::create("", "Arial", 20);
	body_field->setPosition(Size(visibleWidth / 2, visibleHeight / 8 * 3));
	this->addChild(body_field, 2);

    return true;
}

void GameScene::submit_button_click(Ref * pSender) {
	bool isDigit = true;
	if (score_field->getString().length() > 0) {
		if (score_field->getString().length() > 0) {
			for (int i = 0; i < score_field->getString().length(); i++) {
				if (isdigit(score_field->getString()[i]) == false) {
					isDigit = false;
				}
			}
		}

		if (isDigit == true) {
			HttpRequest *request = new HttpRequest();
			request->setRequestType(HttpRequest::Type::POST);
			request->setUrl("http://localhost:8080/submit");
			request->setResponseCallback(CC_CALLBACK_2(GameScene::onHttpRequestCompletedSubmit, this));

			string score = "score=" + score_field->getString();
			const char* postData = score.c_str();
			request->setRequestData(postData, strlen(postData));

			vector<string> headers;
			headers.push_back("Cookie:GAMESESSIONID=" + Global::gameSessionId);
			request->setHeaders(headers);

			cocos2d::network::HttpClient::getInstance()->send(request);
			request->release();
		}
	}
}

void GameScene::rank_button_click(Ref * pSender) {
	HttpRequest *request = new HttpRequest();
	request->setRequestType(HttpRequest::Type::GET);
	request->setUrl("http://localhost:8080/rank?top=10");
	request->setResponseCallback(CC_CALLBACK_2(GameScene::onHttpRequestCompletedRank, this));

	vector<string> headers;
	headers.push_back("Cookie:GAMESESSIONID=" + Global::gameSessionId);
	request->setHeaders(headers);

	cocos2d::network::HttpClient::getInstance()->send(request);
	request->release();
}

void GameScene::onHttpRequestCompletedSubmit(HttpClient *sender, HttpResponse *response) {
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
		if (d.HasMember("info")) {
			score_field->setText(d["info"].GetString());
			database->setStringForKey("header", header);
			database->setStringForKey("body", buffer);
		}
	}
}

void GameScene::onHttpRequestCompletedRank(HttpClient *sender, HttpResponse *response) {
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

	header_field->setText("Header\n" + header + '\n');
	body_field->setText("Body\n" + buffer + '\n');

	rapidjson::Document d;
	d.Parse<0>(buffer.c_str());
	if (d.HasParseError()) {
		CCLOG("GetParseError%s\n", d.GetParseError());
	}

	if (d.IsObject() && d.HasMember("result") && d["result"].GetBool()) {
		if (d.HasMember("info")) {
			string rank = d["info"].GetString();
			for (int i = 0; i < rank.length(); i++) {
				if (rank[i] == '|') rank[i] = '\n';
			}
			rank_field->setText(rank + '\n');
			database->setStringForKey("header", header);
			database->setStringForKey("body", buffer);
		}
	}
}
