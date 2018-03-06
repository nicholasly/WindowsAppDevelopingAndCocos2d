#include "GameSence.h"

USING_NS_CC;

Scene* GameSence::createScene()
{
	// 'scene' is an autorelease object
	auto scene = Scene::create();

	// 'layer1' is an autorelease object
	auto layer1 = GameSence::create();

	// add layer1 as a child to scene
	scene->addChild(layer1);

	// return the scene
	return scene;
}

// on "init" you need to initialize your instance
bool GameSence::init()
{

	if (!Layer::init())
	{
		return false;
	}

	//add touch listener
	EventListenerTouchOneByOne* listener = EventListenerTouchOneByOne::create();
	listener->setSwallowTouches(true);
	listener->onTouchBegan = CC_CALLBACK_2(GameSence::onTouchBegan, this);
	Director::getInstance()->getEventDispatcher()->addEventListenerWithSceneGraphPriority(listener, this);


	Size visibleSize = Director::getInstance()->getVisibleSize();
	Vec2 origin = Director::getInstance()->getVisibleOrigin();

	auto bg = Sprite::create("level-background-0.jpg");
	bg->setPosition(Vec2(visibleSize.width / 2 + origin.x, visibleSize.height / 2 + origin.y));
	this->addChild(bg, 0);

	stoneLayer = Layer::create();
	stoneLayer->setPosition(ccp(0,0));
	this->addChild(stoneLayer, 1);

	stone = Sprite::create("stone.png");
	stone->setPosition(560, 480);
	this->addChild(stone, 1);

	mouseLayer = Layer::create();
	mouseLayer->setPosition(ccp(0, visibleSize.height / 2));
	this->addChild(mouseLayer, 2);

	SpriteFrameCache::getInstance()->addSpriteFramesWithFile("level-sheet.plist");
	char totalFrames = 8;
	char frameName[20];
	Animation* mouseAnimation = Animation::create();
	for (int i = 0; i < totalFrames; i++)
	{
		sprintf(frameName, "gem-mouse-%d.png", i);
		mouseAnimation->addSpriteFrame(SpriteFrameCache::getInstance()->getSpriteFrameByName(frameName));
	}
	mouseAnimation->setDelayPerUnit(0.1);
	AnimationCache::getInstance()->addAnimation(mouseAnimation, "mouseAnimation");

	mouse = Sprite::createWithSpriteFrameName("gem-mouse-0.png");
	Animate* mouseAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("mouseAnimation"));
	mouse->runAction(RepeatForever::create(mouseAnimate));
	mouse->setPosition(ccp(visibleSize.width / 2, visibleSize.height / 2));
	this->addChild(mouse, 2);

	auto shoot = Label::createWithTTF("Shoot", "fonts/arial.ttf", 50.0);
	auto Shoot = MenuItemLabel::create(shoot, CC_CALLBACK_1(GameSence::shootCallback, this));
	Shoot->setPosition(730, 500);
	auto shootMenu = Menu::create(Shoot, NULL);
	shootMenu->setPosition(Vec2(0, 0));
	this->addChild(shootMenu, 1);

	return true;
}

bool GameSence::onTouchBegan(Touch *touch, Event *unused_event) {
	if (cheeze == NULL || cheeze->getPosition() == mouse->getPosition()) {
		auto location = touch->getLocation();
		cheeze = Sprite::create("cheese.png");
		cheeze->setPosition(location);
		this->addChild(cheeze, 2);
		auto mouseMove = MoveTo::create(2, location);
		mouse->runAction(mouseMove);
		auto fadeOut = FadeOut::create(8.0f);
		cheeze->runAction(fadeOut);
	}
	return true;
}

void GameSence::shootCallback(Ref * pSender)
{
	SpriteFrameCache::getInstance()->addSpriteFramesWithFile("level-sheet.plist");
	int totalFrames = 7;
	char frameName[20];
	Animation* diamondAnimation = Animation::create();
	for (int i = 0; i < totalFrames; i++)
	{
		sprintf(frameName, "diamond-%d.png", i);
		diamondAnimation->addSpriteFrame(SpriteFrameCache::getInstance()->getSpriteFrameByName(frameName));
	}
	diamondAnimation->setDelayPerUnit(0.1);
	AnimationCache::getInstance()->addAnimation(diamondAnimation, "diamondAnimation");

	auto diamond = Sprite::createWithSpriteFrameName("pulled-diamond-0.png");
	Animate* diamondAnimate = Animate::create(AnimationCache::getInstance()->getAnimation("diamondAnimation"));
	diamond->runAction(RepeatForever::create(diamondAnimate));
	diamond->setPosition(mouse->getPosition());
	this->addChild(diamond, 2);
		
	auto stonemove = MoveTo::create(1, mouse->getPosition());
	auto fadeOut = FadeOut::create(0.1f);
	auto delay = 2.0;
	auto stoneSeq = Sequence::create(stonemove, fadeOut, nullptr);
	stone->runAction(stoneSeq);
	
	stone = Sprite::create("stone.png");
	stone->setPosition(560, 480);
	this->addChild(stone, 1);

	auto fadeIn = FadeIn::create(0.1f);
	stone->runAction(fadeIn);

	auto moveto = MoveTo::create(1, Vec2((rand() % (900 - 50)) + 50, (rand() % (600 - 50)) + 50));

	auto seq = Sequence::create(moveto, delay);
	mouse->runAction(seq);
}
