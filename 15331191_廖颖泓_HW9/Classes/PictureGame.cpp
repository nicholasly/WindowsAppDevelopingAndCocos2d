#include "AppDelegate.h"
#include "cocos2d.h"
#include "PictureGame.h"
#include "resource.h"

USING_NS_CC;
Scene* PictureGame::createScene()
{
	auto scene = Scene::create();
	auto layer = PictureGame::create();
	scene->addChild(layer);
	return scene;
}

bool PictureGame::init()
{
	if (!Layer::init())
	{
		return false;
	}
	initUI();
	idLarge = false;
	return true;
}

void PictureGame::initUI()
{

	// win size
	auto winSize = Director::getInstance()->getVisibleSize();

	// game bg
	auto bg = Sprite::create(gamebg);
	bg->setPosition(winSize.width / 2, winSize.height / 2);
	bg->setScale(winSize.width / bg->getContentSize().width, winSize.height / bg->getContentSize().height);
	this->addChild(bg);

	// name
	auto *chnStrings = Dictionary::createWithContentsOfFile("String.xml");
	const char *str1 = ((String*)chnStrings->objectForKey("name"))->getCString();
	auto* label1 = Label::create(str1, "Arial", 36);
	label1->setPosition(winSize.width / 2, 270);
	label1->setTag(TAG_LABEL_1);
	this->addChild(label1);

	// id
	const char *str2 = ((String*)chnStrings->objectForKey("id"))->getCString();
	auto* label2 = Label::createWithBMFont("fonts/futura-48.fnt", str2);
	label2->setPosition(winSize.width / 2, 180);
	label2->setTag(TAG_LABEL_2);
	this->addChild(label2);

	// click btn
	auto clickBtn = MenuItemImage::create(click_btn, click_btn_pressed, CC_CALLBACK_1(PictureGame::changeSize, this));
	auto menu = Menu::create(clickBtn, NULL);
	menu->setTag(TAG_CLICK_BTN);
	menu->setPosition(winSize.width / 2, 100);
	this->addChild(menu);

	// update 
	scheduleUpdate();
}

void PictureGame::changeSize(cocos2d::Ref* pSender)
{
	if (idLarge == false) {
		this->getChildByTag(TAG_LABEL_1)->setVisible(false);
		this->getChildByTag(TAG_LABEL_2)->setVisible(false);
		idLarge = true;
	}
	else
	{
		this->getChildByTag(TAG_LABEL_1)->setVisible(true);
		this->getChildByTag(TAG_LABEL_2)->setVisible(true);
		idLarge = false;
	}
}

